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
          
                EnseniarComboBox(obtenerDataComboBox("dbo.Empleados", "nombre"), cbEmpleado);
         
            
                EnseniarComboBox(obtenerMesasDisponibles(), cbMesa);
           
            
        }

        bool bebidas = false;
        

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


        private void btnBebidas_Click(object sender, RoutedEventArgs e)
        {
            bebidas = true;
            lvTipos.Items.Clear();
            EnseniarListaTipoBebidas();
        }

        private void btnPlatos_Click(object sender, RoutedEventArgs e)
        {
            bebidas = false;
            lvTipos.Items.Clear();
            EnseniarListaTipoPlatos();
        }

        private void lvTipos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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

        private void lvSubTipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string script = "";
            //char tipoVendido = 'x';
            //if(lvSubTipos.SelectedValue != null)
            //{
            //    if (bebidas)
            //    {
            //        script = "select idbebida, nombreBebida , Precio from bebidas where  nombreBebida = '" + lvSubTipos.SelectedValue.ToString().Split(" - ")[1].Replace(" ", String.Empty) + "'";
            //        tipoVendido = 'B';

            //    }
            //    else
            //    {
            //        script = "select idPlato, nombrePlato , Precio from Platos where  nombrePlato  = '" + lvSubTipos.SelectedValue.ToString().Split(" - ")[1].Replace(" ", String.Empty) + "'";
            //        tipoVendido = 'P';
            //    }
            //    DataTable dt = new DataTable();
            //    dt = BD.RellenarDataTable(dt, script);

            //    Vendidos vendido = new Vendidos { ID = (int)dt.Rows[0][0], Nombre = (string)dt.Rows[0][1], Precio = float.Parse(dt.Rows[0][2].ToString()) , Tipo = tipoVendido};

            //    dgVendido.Items.Add(vendido);

            //    lblTotal.Content =(Decimal.Parse(lblTotal.Content.ToString()) + (Decimal)vendido.Precio).ToString();
            //}

          
        }

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
            else
            {
                lblTotal.Content = 0;
                dgVendido.Items.Clear();
                cbEmpleado.SelectedIndex = -1;
                cbMesa.SelectedIndex = -1;
            }
        }

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

        private string[] obtenerDataComboBox(string tabla, string columnaNombre)
        {
            string script = "select " + columnaNombre + " from " + tabla + ";";
            string scriptCantidadTipo = "select count(*) as cantidad from " + tabla;

            DataTable dt2 = new DataTable();
            dt2 = BD.RellenarDataTable(dt2, scriptCantidadTipo);
            int cantidadTipos = (int)dt2.Rows[0]["cantidad"];

            string[] tipos = new string[cantidadTipos];

            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0; i < cantidadTipos; i++)
            {
                tipos[i] = (string)dt.Rows[i][columnaNombre];
            }
            return tipos;

        }
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

        private void EnseniarComboBox(string[] tipos, ComboBox cb)
        {
            for (int i = 0; i < tipos.Length; i++)
            {
                cb.Items.Add(tipos[i]);
            }
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            Window menu = new MainWindow();
            menu.ShowDialog();

        }

        private void lvSubTipos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
        }

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

                lblTotal.Content = (Decimal.Parse(lblTotal.Content.ToString()) + (Decimal)vendido.Precio).ToString();
            }

        }
    }
    
    
}
