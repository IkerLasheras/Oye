using Microsoft.Win32;
using Proyecto;
using ProyectoVerano2.Empleados;
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
    public partial class WinInsertarEmpleados : Window
    {
        /// <summary>
        /// Inicializa los componentes, rellena el TextView del ID con un 0, 
        /// Rellena los TextBoxes con la informacion de este ID usando el metodo de EneniarEnPantalla(), 
        /// Enseña todos los tipos Bebidas en el comboBox. 
        /// </summary>
        public WinInsertarEmpleados()
        {
            
            InitializeComponent();
            if(txtID.Text == "")
            {
                txtID.Text = "0";

            }
            EnseniarEmpleadoEnPantalla(Int32.Parse(txtID.Text));
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.TipoEmpleado" , "nombreTipoEmpleado"),cbTipo);
        }

        bool insertoImagen = false;

        /// <summary>
        /// Recoge los Datos de los TextBoxes y se insertan en la base de datos.
        /// En el caso que se vaya a insertar una imagen. Se hace una modificacion en la base de datos
        /// con el mismo valor de ID que el insertado para asi insertar la imagen.
        /// </summary>
        public void InsertarDatosBBDD()
        {
            string comandMod =
                "INSERT INTO [dbo].[Empleados] " +
                "(" +
                "[idEmpleado]," +
                "[nombre]," +
                "[apellido1]," +
                "[apellido2]," +
                "[dni]," +
                "[telefono]" +
                ",[email]," +
                "[fechNac]" +
                ",[localidad]" +
                ",[direccion]" +
                ",[cp]" +
                ",[tipoEmpleado]" +
                ",[cuentaCredito]" +
                ",[fechIncicioContrato])" +
                "VALUES" +
                "(" +
                "@idEmpleado" +
                ", @nombre" +
                ", @apellido1" +
                ", @apellido2" +
                ", @dni" +
                ", @telefono" +
                ", @email" +
                ", @fechNac" +
                ", @localidad" +
                ", @direccion" +
                ", @cp" +
                ", @tipoEmpleado" +
                ", @cuentaCredito" +
                ", @fechIncicioContrato);";

            string scriptImg = "UPDATE [dbo].[Empleados] SET [imgEmpleado] = @imgEmpleado WHERE idEmpleado = @idEmpleadoImg";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            string[] apellidos = txtApellidos.Text.Split(' ');
            string apellido2 = "";

            if (apellidos.Length != 1)
            {
                apellido2 = apellidos[1];
            }
            DateTime fechaNacimiento = DateTime.ParseExact(dpFechNac.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicio = DateTime.ParseExact(dpFInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            listaParametros.Add(BD.ObtenerParametro("@idEmpleado", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombre", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@apellido1", SqlDbType.NChar, ParameterDirection.Input, false, apellidos[0]));
            listaParametros.Add(BD.ObtenerParametro("@apellido2", SqlDbType.NChar, ParameterDirection.Input, true, apellido2));
            listaParametros.Add(BD.ObtenerParametro("@dni", SqlDbType.NChar, ParameterDirection.Input, false, txtDni.Text));
            listaParametros.Add(BD.ObtenerParametro("@telefono", SqlDbType.NChar, ParameterDirection.Input, true, txtMovil.Text));
            listaParametros.Add(BD.ObtenerParametro("@email", SqlDbType.NChar, ParameterDirection.Input, true, txtEmail.Text));
            listaParametros.Add(BD.ObtenerParametro("@fechNac", SqlDbType.Date, ParameterDirection.Input, true, fechaNacimiento));
            listaParametros.Add(BD.ObtenerParametro("@localidad", SqlDbType.NChar, ParameterDirection.Input, true, txtLocalidad.Text));
            listaParametros.Add(BD.ObtenerParametro("@direccion", SqlDbType.NChar, ParameterDirection.Input, true, txtDireccion.Text));
            listaParametros.Add(BD.ObtenerParametro("@cp", SqlDbType.Int, ParameterDirection.Input, true, 50014));
            listaParametros.Add(BD.ObtenerParametro("@cuentaCredito", SqlDbType.NChar, ParameterDirection.Input, false, txtFIBAN.Text));
            listaParametros.Add(BD.ObtenerParametro("@tipoEmpleado", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@fechIncicioContrato", SqlDbType.Date, ParameterDirection.Input, true, fechaInicio));

          


            BD.LanzarComandoSQLNonQuery(comandMod, listaParametros);

            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@idEmpleadoImg", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
                listaParametrosImg.Add(BD.ObtenerParametro("@imgEmpleado", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
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
        /// Obtiene imagen desde la base de datos para así poder imprimirla en pantalla
        /// </summary>
        /// <param name="dt">DataTable en la que se encuentra la Imagen de la bebida </param>
        /// <param name="id">Id de la bebida que queremos obtener la imagen</param>
        /// <returns>Objeto de mapa de Bits</returns>
        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {
            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgEmpleado"]))
            {
                var result = dt.Rows[id]["imgEmpleado"];
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
        /// <param name="id">Id del empleado a mostrar en pantalla</param>
        public void EnseniarEmpleadoEnPantalla(int id)
        {
            txtID.Text = id.ToString();
            string script = "SELECT * FROM dbo.Empleados;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombre"];
                txtApellidos.Text = (string)dt.Rows[id]["apellido1"] + " " + (string)dt.Rows[0]["apellido2"];
                dpFechNac.Text = ((DateTime)dt.Rows[id]["fechNac"]).ToString("dd/MM/yyyy");
                txtLocalidad.Text = (string)dt.Rows[id]["localidad"];
                txtCP.Text = dt.Rows[id]["cp"].ToString();
                txtEmail.Text = (string)dt.Rows[id]["email"];
                txtDireccion.Text = (string)dt.Rows[id]["direccion"];
                dpFInicio.Text = ((DateTime)dt.Rows[id]["fechIncicioContrato"]).ToString("dd/MM/yyyy");
                txtFIBAN.Text = (string)dt.Rows[id]["cuentaCredito"];
                txtMovil.Text = (string)dt.Rows[id]["telefono"];
                cbTipo.SelectedIndex = (int)dt.Rows[id]["tipoEmpleado"];
                txtDni.Text = (string)dt.Rows[id]["dni"];
                imgEmpleado.Source = ObtenerImgBBDD(dt, id);

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
            TextBox[] textBoxes = { txtNombre, txtApellidos, txtLocalidad, txtCP, txtEmail, txtDireccion,txtFIBAN, txtMovil, txtDni };

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            imgEmpleado.Source = null;
            cbTipo.SelectedIndex = 0;
        }

        /// <summary>
        /// Realiza el Update los datos de la base de datos dependiendo del id que aparece en su TextBox
        /// </summary>
        private void UploadEmpleado()
        {
            string script = "UPDATE [dbo].[Empleados]" +
                " SET [nombre] = @nombre " +
                ",[apellido1] = @apellido1 " +
                ",[apellido2] = @apellido2 " +
                ",[dni] = @dni " +
                ",[telefono] = @telefono " +
                ",[email] = @email " +
                ",[fechNac] = @fechNac " +
                ",[localidad] = @localidad " +
                ",[direccion] = @direccion " +
                ",[cp] = @cp " +
                ",[tipoEmpleado] = @tipoEmpleado " +
                ",[cuentaCredito] = @cuentaCredito " +
                ",[fechIncicioContrato] = @fechIncicioContrato " +
                "WHERE idEmpleado = @idEmpleado;";

            string scriptImg = "UPDATE [dbo].[Empleados] SET [imgEmpleado] = @imgEmpleado WHERE idEmpleado = @idEmpleadoImg";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            string[] apellidos = txtApellidos.Text.Split(' ');
            string apellido2 = "";

            if (apellidos.Length != 1)
            {
                apellido2 = apellidos[1];
            }
            DateTime fechaNacimiento = DateTime.ParseExact(dpFechNac.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicio = DateTime.ParseExact(dpFInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            listaParametros.Add(BD.ObtenerParametro("@idEmpleado", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombre", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@apellido1", SqlDbType.NChar, ParameterDirection.Input, false, apellidos[0]));
            listaParametros.Add(BD.ObtenerParametro("@apellido2", SqlDbType.NChar, ParameterDirection.Input, true, apellido2));
            listaParametros.Add(BD.ObtenerParametro("@dni", SqlDbType.NChar, ParameterDirection.Input, false, txtDni.Text));
            listaParametros.Add(BD.ObtenerParametro("@telefono", SqlDbType.NChar, ParameterDirection.Input, true, txtMovil.Text));
            listaParametros.Add(BD.ObtenerParametro("@email", SqlDbType.NChar, ParameterDirection.Input, true, txtEmail.Text));
            listaParametros.Add(BD.ObtenerParametro("@fechNac", SqlDbType.Date, ParameterDirection.Input, true, fechaNacimiento));
            listaParametros.Add(BD.ObtenerParametro("@localidad", SqlDbType.NChar, ParameterDirection.Input, true, txtLocalidad.Text));
            listaParametros.Add(BD.ObtenerParametro("@direccion", SqlDbType.NChar, ParameterDirection.Input, true, txtDireccion.Text));
            listaParametros.Add(BD.ObtenerParametro("@cp", SqlDbType.Int, ParameterDirection.Input, true, 50014));
            listaParametros.Add(BD.ObtenerParametro("@cuentaCredito", SqlDbType.NChar, ParameterDirection.Input, false, txtFIBAN.Text));
            listaParametros.Add(BD.ObtenerParametro("@tipoEmpleado", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@fechIncicioContrato", SqlDbType.Date, ParameterDirection.Input, true, fechaInicio));
            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@idEmpleadoImg", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
                listaParametrosImg.Add(BD.ObtenerParametro("@imgEmpleado", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
            }

        insertoImagen = false;

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        /// <summary>
        /// Abre nueva ventana para crear un nuevo tipo de Empleado.
        /// Al cerrar esta, se mostrara el los tipos en el Combobox de nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearTipo_Click(object sender, RoutedEventArgs e)
        {
            Window crearTipoEmpleado = new CrearTipoEmpleado();
            crearTipoEmpleado.ShowDialog();
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.TipoEmpleado", "nombreTipoEmpleado"), cbTipo);
        }

        /// <summary>
        /// Evento de click en el boton de cambiar, Realiza la modificacion de la pase de datos mediante el metodo de Modificar()
        /// </summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            UploadEmpleado();
            insertoImagen = false;
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
            BD.InsertarImagen(imgEmpleado, lblUrl, insertoImagen);

        }

        /// <summary>
        /// Evento de click en el boton de nuevo, realiza una consulta a la BBDD del maximo ID mediante el metodo BuscarIdmax
        /// , borra el contenido de los TextBoxes e intercambia el boton de grabar por el de cambiar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idEmpleado", "Empleados", txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Evento de click en el boton de Guardar, realiza la insercción de los los datos mediante el metodo InsertarDatosBBDD()
        /// Intercambia los botones Cambiar y Guardar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatosBBDD();
            insertoImagen = false;

            BD.NoEnseniarBoton(btnGuardar);
            BD.EnseniarBoton(btnCambiar);
        }

        /// <summary>
        /// Evento de click del boton Listado. Abre la ventana de listas, al cerrar esta se rellenan
        /// los TextBoxes dependiendo del ID de ListaEmpleados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window listado = new Lista();
            listado.ShowDialog();

            EnseniarEmpleadoEnPantalla(Empleados.Lista.id);


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
        /// y rellena los textBoxes con la informaciónd e dicho Id con el metodo de EnseniaEmpleadoEnPantalla 
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarEmpleadoEnPantalla(Int32.Parse(txtID.Text));
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
                EnseniarEmpleadoEnPantalla(Int32.Parse(txtID.Text));
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }


    }
}
