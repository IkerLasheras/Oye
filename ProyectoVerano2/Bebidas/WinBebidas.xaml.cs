using Microsoft.Win32;
using Proyecto;
using ProyectoVerano2.Bebidas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProyectoVerano2
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class WinBebidas : Window
    {
        /// <summary>
        /// Inicializa los componentes, rellena el TextView del ID con un 0, 
        /// Rellena los TextBoxes con la informacion de este ID usando el metodo de EneniarEnPantalla(), 
        /// Enseña todos los tipos Bebidas en el comboBox. 
        /// </summary>
        public WinBebidas()
        {
            InitializeComponent();
            if (txtID.Text == "")
            {
                txtID.Text = "0";
            }
            EnseniarEnPantalla(Int32.Parse(txtID.Text));
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.TipoBebida", "nombre"), cbTipo);
        }

        bool insertoImagen = false;

        /// <summary>
        /// Recoge los Datos de los TextBoxes y se insertan en la base de datos.
        /// En el caso que se vaya a insertar una imagen. Se hace una modificacion en la base de datos
        /// con el mismo valor de ID que el insertado para asi insertar la imagen.
        /// </summary>
        public void InsertarDatos()
        {
            string script =
                "INSERT INTO [dbo].[Bebidas] " +
                "(" +
                "[idBebida]," +
                "[precio]," +
                "[tipoBebida]," +
                "[nombreBebida])"+
                "VALUES" +
                "(" +
                "@idBebida" +
                ", @precio" +
                ", @tipoBebida" +
                ", @nombreBebida);";
               
            string scriptImagen = "UPDATE [dbo].[Bebidas] SET [imgBebida] = @imgBebida WHERE [idBebida] = @idBebida;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@precio", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtPrecio.Text)));
            listaParametros.Add(BD.ObtenerParametro("@tipoBebida", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));

            listaParametrosImg.Add(BD.ObtenerParametro("@imgBebida", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
            listaParametrosImg.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);

            if (insertoImagen)
            {
                BD.LanzarComandoSQLNonQuery(scriptImagen, listaParametrosImg);
            }
        }

        /// <summary>
        /// Coge el contenido de la etiqueta url y lo coloca en el Path del control de imagen.
        /// </summary>
        /// <returns>Devuelbe una imagen</returns>
        private ImageClass CambiarFotoParaBBDD()
        {
            ImageClass images = new ImageClass();

            images.ImagePath = (string)lblUrl.Content;

            if (lblUrl.Content != "")
            {
                images.imagen = File.ReadAllBytes((string)lblUrl.Content);
            }
            else
            {
                images.imagen = null;
              
            }
            return images;
        }

        /// <summary>
        /// Cambia el valor booleano de insertoImagen (Se usará para el control de insercción de imagen),
        /// Realiza la insercción mediante el metodo de InsertarImagen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnInsertarImg_Click(object sender, RoutedEventArgs e)
        {
            insertoImagen = true;
            BD.InsertarImagen(imgBebida,lblUrl,insertoImagen);
        }

        /// <summary>
        /// Obtiene imagen desde la base de datos para así poder imprimirla en pantalla
        /// </summary>
        /// <param name="dt">DataTable en la que se encuentra la Imagen de la bebida </param>
        /// <param name="id">Id de la bebida que queremos obtener la imagen</param>
        /// <returns>Objeto de mapa de Bits</returns>
        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {
            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgBebida"]))
            {
                var result = dt.Rows[id]["imgBebida"];
                Stream StreamObj = new MemoryStream((byte[])result);

                BitmapImage BitObj = new BitmapImage();

                BitObj.BeginInit();

                BitObj.StreamSource = StreamObj;

                BitObj.EndInit();

                return BitObj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Rellena los Tex Boxes con la información de un id dado
        /// </summary>
        /// <param name="id">Id de la bebida a mostrar en pantalla</param>
        private void EnseniarEnPantalla(int id)
        {
            txtID.Text = id.ToString();
            string script = "SELECT * FROM dbo.Bebidas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreBebida"];
                txtPrecio.Text = dt.Rows[id]["precio"].ToString();
                cbTipo.SelectedIndex = (int)dt.Rows[id]["tipoBebida"];
                
                imgBebida.Source = ObtenerImgBBDD(dt, id);

                BD.EnseniarBoton(btnCambiar);
                BD.NoEnseniarBoton(btnGuardar);
            }
            else
            {
                BD.EnseniarBoton(btnGuardar);
                BD.NoEnseniarBoton(btnCambiar);
                txtID.Text = "0";
            }
        }

        /// <summary>
        /// Deja vacios todo los controles excepto el del ID
        /// </summary>
        private void vaciarCeldas()
        {
            TextBox[] textBoxes = { txtNombre, txtPrecio};
            ComboBox[] comboBoxes = { cbTipo };
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            imgBebida.Source = null;

        }

        /// <summary>
        /// Realiza el Update los datos de la base de datos dependiendo del id que aparece en su TextBox
        /// </summary>
        private void Modificar()
        {
            string script = "UPDATE [dbo].[Bebidas]" +
                " SET [nombreBebida] = @nombreBebida " +
                ",[precio] = @precio " +
                ",[tipoBebida] = @tipoBebida " +
                "where idBebida = @idBebida ";

            string scriptImg = "UPDATE [dbo].[Bebidas] SET [imgBebida] = @imgBebida WHERE [idBebida] = @idBebida";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();


            listaParametros.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@precio", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtPrecio.Text)));
            listaParametros.Add(BD.ObtenerParametro("@tipoBebida", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));

            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@imgBebida", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                listaParametrosImg.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
            }
            insertoImagen = false;

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        /// <summary>
        /// Evento de click en el boton de Guardar, realiza la insercción de los los datos mediante el metodo InsertarDatos
        /// Intercambia los botones Cambiar y Guardar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatos();
            insertoImagen = false;

            BD.NoEnseniarBoton(btnGuardar);
            BD.EnseniarBoton(btnCambiar);
        }

        /// <summary>
        /// Evento de click en el boton de cambiar, Realiza la modificacion de la pase de datos mediante el metodo de Modificar()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            Modificar();
            insertoImagen = false;
        }

        /// <summary>
        /// Evento de click en el boton de nuevo, realiza una consulta a la BBDD del maximo ID mediante el metodo BuscarIdmax
        /// , borra el contenido de los TextBoxes e intercambia el boton de grabar por el de cambiar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idBebida", "Bebidas", txtID);
            vaciarCeldas();

            BD.NoEnseniarBoton(btnCambiar);
            BD.EnseniarBoton(btnGuardar);
        }

        /// <summary>
        /// Abre nueva ventana para crear un nuevo tipo de Bebida.
        /// Al cerrar esta, se mostrara el los tipos en el Combobox de nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearTipo_Click(object sender, RoutedEventArgs e)
        {
            Window crearTipoBebida = new WinTipoBebida();
            crearTipoBebida.ShowDialog();
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.TipoBebida", "nombre"), cbTipo);
           
        }

        /// <summary>
        /// Evento de click del boton Listado. Abre la ventana de listas, al cerrar esta se rellenan
        /// los TextBoxes dependiendo del ID de ListasBebidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window lista = new Bebidas.ListaBebidas();
            lista.ShowDialog();
            EnseniarEnPantalla(ListaBebidas.id);
        }

        /// <summary>
        /// Evento de click en el boton de Salir. Cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Evento de click en el boton de siguiente. Incrementa el id mediante el metodo IncrementarID() 
        /// y rellena los textBoxes con la informaciónd e dicho Id con el metodo de EnseniaEnPantalla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarEnPantalla(Int32.Parse(txtID.Text));
            }
            catch (Exception ex)
            {
                BD.DisminuirID(txtID);
            }
        }

        /// <summary>
        /// Evento de click en el boton de Anterior. Disminuye el id mediante el mediante el metodo DisminuirID()
        /// y rellena los textBoxes con la informaciónd e dicho Id con el metodo de EnseniarEnPantalla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.DisminuirID(txtID);
                EnseniarEnPantalla(Int32.Parse(txtID.Text));
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }

    }
}
