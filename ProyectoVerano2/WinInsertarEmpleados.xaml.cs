using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using Proyecto;
using ImageBBDD;
using System.IO;

using Microsoft.Win32;
using System.Data.Entity;
using System.Drawing;

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
            BuscarIdMax("idEmpleado", "dbo.Empleados");

        }
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void BuscarIdMax(string columna, string tabla)
        {
            int id = 0;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PruebaCasa"].ConnectionString;
            string script = "SELECT max(idEmpleado) FROM dbo.Empleados;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script, connectionString);

          

           id = (int)dt.Rows[0][0] + 1;

            txtID.Text = id.ToString();

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
                ",[fechIncicioContrato]" +
                ",[imgEmpleado])" +
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
                ", @fechIncicioContrato" +
                ", @imgEmpleado);";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PruebaCasa"].ConnectionString;

            List<SqlParameter> listaParametros = new List<SqlParameter>();

            string[] apellidos = txtApellidos.Text.Split(' ');
            string apellido2 = "";

            if (apellidos.Length != 1)
            {
                apellido2 = apellidos[1];
            }
            DateTime fechaNacimiento = DateTime.ParseExact(txtFechNac.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicio = DateTime.ParseExact(txtFechInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);


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
            listaParametros.Add(BD.ObtenerParametro("@tipoEmpleado", SqlDbType.Int, ParameterDirection.Input, true, 1));
            listaParametros.Add(BD.ObtenerParametro("@cuentaCredito", SqlDbType.NChar, ParameterDirection.Input, false, txtFIBAN.Text));
            listaParametros.Add(BD.ObtenerParametro("@fechIncicioContrato", SqlDbType.Date, ParameterDirection.Input, true, fechaInicio));
            listaParametros.Add(BD.ObtenerParametro("@imgEmpleado", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));

            BD.LanzarComandoSQLNonQuery(comandMod, connectionString, listaParametros);

            Close();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatosBBDD();
        }

        private ImageClass CambiarFotoParaBBDD()
        {
            ImageClass images = new ImageClass();

            images.ImagePath = (string)lblUrl.Content;

            if (lblUrl.Content != "")
            {
                images.imagen = File.ReadAllBytes((string)lblUrl.Content);
            }else
            {
                images.imagen = null;
;
            }

           

            return images;

        }

        private async void btnInsertarImg_Click(object sender, RoutedEventArgs e)
        {

            { 
                //ImageClass images = new ImageClass();

                OpenFileDialog openFileDialog1 = new OpenFileDialog();


                openFileDialog1.Filter = "Archivos de imágenes (*.bmp, *.jpg)|*.bmp;*.jpg|Todos los archivos (*.*)|*.*"; 

                openFileDialog1.DefaultExt = ".jpeg";

                openFileDialog1.ShowDialog();

                lblUrl.Content = openFileDialog1.FileName;

                ImageSource imageSource = new BitmapImage(new Uri((string)lblUrl.Content));

                imgEmpleado.Source = imageSource;

            }

        }

    }

}
