using Microsoft.Win32;
using Proyecto;
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

        public WinInsertarEmpleados()
        {
            
            InitializeComponent();
            txtID.Text = "0";
            EnseniarEmpleadoEnPantalla();
            EnseniarTipoEmpleado(TiposEmpleado());

            
        }

        bool insertoImagen = false;
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarEmpleadoEnPantalla();
            }
            catch (Exception ex)
            {
                BD.DisminuirID(txtID);
            }
        }

        private TextBox GetTxtID()
        {
            return txtID;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
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

            string scriptImagen = "INSERT INTO [dbo].[Empleados] ([imgEmpleado]) VALUES (@imgEmpleado);";

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

            listaParametrosImg.Add(BD.ObtenerParametro("@imgEmpleado", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));


            BD.LanzarComandoSQLNonQuery(comandMod, listaParametros);

            if (insertoImagen)
            {
                BD.LanzarComandoSQLNonQuery(scriptImagen, listaParametrosImg);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatosBBDD();
            insertoImagen = false;

            BD.NoEnseniarBoton(btnGuardar);
            BD.EnseniarBoton(btnCambiar);
        }

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

        private async void btnInsertarImg_Click(object sender, RoutedEventArgs e)
        {
            BD.InsertarImagen(imgEmpleado,lblUrl,insertoImagen);
        }

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

        private void EnseniarEmpleadoEnPantalla()
        {
            int id = Int32.Parse(txtID.Text);
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

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idEmpleado", "Empleados" , txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

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

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.DisminuirID(txtID);
                EnseniarEmpleadoEnPantalla();
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }

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

        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            UploadEmpleado();
            insertoImagen = false;
        }

        private void btnCrearTipo_Click(object sender, RoutedEventArgs e)
        {
            Window crearTipoEmpleado = new CrearTipoEmpleado();
            crearTipoEmpleado.ShowDialog();
        }

        private string[] TiposEmpleado()
        {
            string script = "select nombreTipoEmpleado from tipoEmpleado";
            string scriptCantidadTipo = "select count(*) as cantidad from TipoEmpleado";

            DataTable dt2 = new DataTable();
            dt2 = BD.RellenarDataTable(dt2, scriptCantidadTipo);
            int cantidadTipos = (int)dt2.Rows[0]["cantidad"];

            string[] tipos = new string[cantidadTipos];

            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);
            
            for(int i = 0; i<cantidadTipos; i++)
            {
                tipos[i] = (string)dt.Rows[i]["nombreTipoEmpleado"];
            }
            return tipos;
        }
        private void EnseniarTipoEmpleado(string[] tipos)
        {
            for (int i = 0; i < tipos.Length; i++)
            {
                cbTipo.Items.Add(tipos[i]);
            }
        }
    }
}
