using Proyecto;
using ProyectoVerano2.Platos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoVerano2.TPV
{
    /// <summary>
    /// Lógica de interacción para TPV.xaml
    /// </summary>
    public partial class TPV2 : Window
    {
        public TPV2()
        {
            InitializeComponent();
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.Empleados", "nombre"), cbEmpleado);
            BD.EnseniarComboBox(obtenerMesasDisponibles(), cbMesa);
        }

        bool bebidas = false;
        
        /// <summary>
        /// Rellena el ListView con los platos que pertenecen al tipo de plato insertado.
        /// </summary>
        /// <param name="tipoPlato">Id del tipo de plato</param>
        public void EnseniarListaPlatosPorTipo(int tipoPlato)
        {
            string script = "select idPlato, nombrePlato from platos where tipoPlato = " + tipoPlato ;
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

                for(int i = 0; i<dt.Rows.Count; i++)
                {
                    lvSubTipos.Items.Add(dt.Rows[i][0] + " - " + dt.Rows[i][1]);
                }
        }

        /// <summary>
        /// Rellena el listView de tipos con los tipos de platos que existen.
        /// </summary>
        public void EnseniarListaTipoPlatos()
        {
            string script = "select nombreTipo from tipoPlatos";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lvTipos.Items.Add(dt.Rows[i][0]);
            }
        }

        /// <summary>
        /// Rellena el ListView con las bebidas que pertenecen al tipo de bebida insertado.
        /// </summary>
        /// <param name="tipoBebidas"></param>
        public void EnseniarListaBebidasPorTipo(int tipoBebidas)
        {
            string script = "select idBebida, nombreBebida from bebidas where tipobebida = " + tipoBebidas ;
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lvSubTipos.Items.Add(dt.Rows[i][0] + " - " + dt.Rows[i][1]);
            }
        }

        /// <summary>
        /// Rellena el listView de tipos con los tipos de bebidas que existen.
        /// </summary>
        public void EnseniarListaTipoBebidas()
        {
            string script = "select nombre from tipoBebida";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lvTipos.Items.Add(dt.Rows[i][0]);
            }
        }

        /// <summary>
        /// Evento de click en el boton de bebidas. Limpia la ListView y lista todos los tipos en el ListView Subtipos con el metodo EnseniarListaTipoBebidas().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBebidas_Click(object sender, RoutedEventArgs e)
        {
            bebidas = true;
            lvTipos.Items.Clear();
            EnseniarListaTipoBebidas();
        }

        /// <summary>
        /// Evento de click en el boton de Platos. Limpia la ListView y lista todos los tipos en el ListView Subtipos con el metodo EnseniarListaTipoPlatos().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlatos_Click(object sender, RoutedEventArgs e)
        {
            bebidas = false;
            lvTipos.Items.Clear();
            EnseniarListaTipoPlatos();
        }

        /// <summary>
        /// Evento de click del boton salir. Cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Al clicar uno de los tipos de platos o bebidas aparecen todos los platos/bebidas pertenecientes a este con el metodo EnseniarListaBebidasPorTipo(lvTipos.SelectedIndex);
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvTipos_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            lvSubTipos.Items.Clear();

            if (bebidas)
            {
                EnseniarListaBebidasPorTipo(lvTipos.SelectedIndex);
            }
            else
            {
                EnseniarListaPlatosPorTipo(lvTipos.SelectedIndex);
            }

        }

        /// <summary>
        /// Evento de click del boton Guardar. Muestra MessageBox, en caso de responder Ok , se insertan los datos del pedido tanto en la tabla Pedidos como en la de DetallesPedidos usando sus repectivos metodos,
        /// tras esto se borra todos los datos del pedido en pantalla para ralizar uno nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = System.Windows.MessageBox.Show("¿Quieres guardar el Pedido?", "Pedido", MessageBoxButton.OKCancel);
            if(result.HasFlag(MessageBoxResult.OK))
            {
                int id = BD.BuscarUltimoPedido();

                insertarPedido(id);
                insertarDescripcionPedido(id);

                lblTotal.Content = 0;
                dgVendido.Items.Clear();
                cbEmpleado.SelectedIndex= -1;
                cbMesa.SelectedIndex= -1;
            }
        }

        /// <summary>
        /// Se inseta los datos necesarios en la tabla Pedidos
        /// </summary>
        /// <param name="id">ID del pedido a insertar (ID Maximo en perdidos + 1)</param>
        public void insertarPedido(int id)
        {
            string script = "INSERT INTO [dbo].[Pedidos] " +
                "(" +
                 "[idMesa]," +
                 "[idEmpleado]," +
                 "[fechaPedido]," +
                 "[costeTotal]," +
                "[idPedidos])" +
                "VALUES" +
                "(" +
                "@idMesa" +
                ",@idEmpleado" +
                ",@fechaPedido" +
                ",@costeTotal" +
                ",@idPedidos);";

            List<SqlParameter> listaParametros = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idMesa", SqlDbType.Int, ParameterDirection.Input, true, cbMesa.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@idEmpleado", SqlDbType.Int, ParameterDirection.Input, false, cbEmpleado.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@fechaPedido", SqlDbType.Date, ParameterDirection.Input, false, DateTime.Now));
            listaParametros.Add(BD.ObtenerParametro("@idPedidos", SqlDbType.Int, ParameterDirection.Input, false, id));
            listaParametros.Add(BD.ObtenerParametro("@costeTotal", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(lblTotal.Content.ToString())));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        /// <summary>
        /// Se inserta los datos sacados del DataGrid de los pedidos. 
        /// </summary>
        /// <param name="id">ID que lo relaciona con el Pedido</param>
        public void insertarDescripcionPedido(int id)
        { 
            string script = "INSERT INTO [dbo].[DescripcionPedido] " +
                "(" +
                 "[idPedido]," +
                 "[idVendido]," +
                "[tipo])" +
                "VALUES" +
                "(" +
                "@idPedido" +
                ",@idVendido" +
                ",@tipo);";

            foreach(Vendidos vendidos in dgVendido.Items)
            {
                List<SqlParameter> listaParametros = new List<SqlParameter>();

                listaParametros.Add(BD.ObtenerParametro("@idPedido", SqlDbType.Int, ParameterDirection.Input, true, id));
                listaParametros.Add(BD.ObtenerParametro("@idVendido", SqlDbType.Int, ParameterDirection.Input, false, vendidos.ID));
                listaParametros.Add(BD.ObtenerParametro("@tipo", SqlDbType.Char, ParameterDirection.Input, false, vendidos.Tipo));

                BD.LanzarComandoSQLNonQuery(script, listaParametros);
            }       
        }

        /// <summary>
        /// Muestra en el combobox todas las mesas disponibles
        /// </summary>
        /// <returns></returns>
        private string[] obtenerMesasDisponibles()
        {
            string script = "select idMesa from Mesas where disponible = 1;";
            string scriptCantidadTipo = "select count(*) as cantidad from  Mesas where disponible =1";

            DataTable dt2 = new DataTable();
            dt2 = BD.RellenarDataTable(dt2, scriptCantidadTipo);
            int cantidadTipos = (int)dt2.Rows[0]["cantidad"];

            string[] tipos = new string[cantidadTipos];

            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0; i < cantidadTipos; i++)
            {
                tipos[i] = dt.Rows[i]["idMesa"].ToString();
            }
            return tipos;
        }

        /// <summary>
        /// Al clicar en un objeto de dentro de la lista de subtipos, manda el id de este, su nombre, su precio y un caracter para diferenciar entre Bebidas(B) y Platos(P) a un DataGrid que lo muestra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSubTipos_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string script = "";
            char tipoVendido = 'x';
            if (lvSubTipos.SelectedValue != null)
            {
                if (bebidas)
                {
                    script = "select idbebida, nombreBebida , Precio from bebidas where  nombreBebida = '" + lvSubTipos.SelectedValue.ToString().Split(" - ")[1].Replace(" ", String.Empty) + "'";
                    tipoVendido = 'B';

                }
                else
                {
                    script = "select idPlato, nombrePlato , Precio from Platos where  nombrePlato  = '" + lvSubTipos.SelectedValue.ToString().Split(" - ")[1].Replace(" ", String.Empty) + "'";
                    tipoVendido = 'P';
                }
                DataTable dt = new DataTable();
                dt = BD.RellenarDataTable(dt, script);

                Vendidos vendido = new Vendidos { ID = (int)dt.Rows[0][0], Nombre = (string)dt.Rows[0][1], Precio = float.Parse(dt.Rows[0][2].ToString()), Tipo = tipoVendido };

                dgVendido.Items.Add(vendido);

                lblTotal.Content = (Decimal.Parse(lblTotal.Content.ToString()) + (Decimal)vendido.Precio).ToString(".00");
            }

        }
    }
    
    
}
