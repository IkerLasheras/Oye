using Proyecto;
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

namespace ProyectoVerano2
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class CrearTipoEmpleado : Window
    {
        public CrearTipoEmpleado()
        {
            InitializeComponent();
            BD.BuscarIDMax("idTipoEmpleado", "dbo.TipoEmpleado",txtID);
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarTipoEmpleadoEnPantalla();
            }
            catch (Exception ex)
            {
                BD.DisminuirID(txtID);
                
            }
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            InsertarEnBBDD();
        }

        private void InsertarEnBBDD()
        {
            string scriptTipoEmpleado = "INSERT INTO [dbo].[TipoEmpleado] ([idTipoEmpleado],[nombreTipoEmpleado],[sueldoBase]) VALUES (@idTipoEmpleado, @nombreTipoEmpleado, @sueldoBase);";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PruebaCasa"].ConnectionString;

            List<SqlParameter> listaParametros = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idTipoEmpleado", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreTipoEmpleado", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@sueldoBase", SqlDbType.Decimal, ParameterDirection.Input, false, txtSueldo.Text));

            BD.LanzarComandoSQLNonQuery(scriptTipoEmpleado,listaParametros);
        }
        private void EnseniarTipoEmpleadoEnPantalla()
        {
            int id = Int32.Parse(txtID.Text);
            string script = "SELECT * FROM dbo.TipoEmpleado;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombre"];
                txtSueldo.Text = (string)dt.Rows[id]["sueldoBase"];
            }
            else
            {
                txtID.Text = "0";
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.DisminuirID(txtID);
                EnseniarTipoEmpleadoEnPantalla();
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }
    }
}
