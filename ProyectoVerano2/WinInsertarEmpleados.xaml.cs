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
using System.IO;
using Windows.Storage;
using Microsoft.Win32;

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

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["proyecto"].ConnectionString;
            string script = "SELECT Max(idEmpleado) FROM dbo.Empleados";
            DataTable dt = new DataTable();
            BD.RellenarDataTable(dt, script, connectionString);

            if (!dt.HasErrors)
            {
                id = 0;
            }
            else
            {
                DataRow row = dt.Rows[0];
                id = (int)row["idEmpleado"];
            }
            txtID.Text = id.ToString();

        }
        public void InsertarDatosBBDD()
        {
            string comandMod =
                "INSERT INTO [dbo].[Empleados] " +
                "([nombre]," +
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
                "(@nombre" +
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

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["proyecto"].ConnectionString;

            List<SqlParameter> listaParametros = new List<SqlParameter>();

            string[] apellidos = txtApellidos.Text.Split(' ');
            string apellido2 = "";

            if (apellidos.Length != 0)
            {
                apellido2 = "apellido[1]";
            }
            DateTime fechaNacimiento = DateTime.ParseExact(txtFechNac.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicio = DateTime.ParseExact(txtFechInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

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
            listaParametros.Add(BD.ObtenerParametro("@imgEmpleado", SqlDbType.Image, ParameterDirection.Input, false, imgEmpleado));

            BD.LanzarComandoSQLNonQuery(comandMod, connectionString, listaParametros);

            Close();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatosBBDD();
        }

        private async void btnInsertarImg_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image files | *.jpg";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtStudentImage.Text = openFileDialog1.FileName;
                    pbStudentImage.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
    }

}
