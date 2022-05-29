using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    public partial class WinUbicacionProducto : Window
    {
        bool insertoImagen = false;
        public WinUbicacionProducto()
        {
            InitializeComponent();
            txtID.Text = "0";
            EnseniarTipoEmpleadoEnPantalla();

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
            insertoImagen = false;
            BD.NoEnseniarBoton(btnGrabar);
            BD.EnseniarBoton(btnCambiar);
        }

        private void InsertarEnBBDD()
        {
            string script = "INSERT INTO [dbo].[Ubicacion] ([idUbicacion],[descripcionUbicacion],[nombreUbicacion]) VALUES (@idUbicacion, @descripcionUbicacion, @nombreUbicacion);";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idUbicacion", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@descripcionUbicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtDescripcion.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreUbicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }
    
    private void EnseniarTipoEmpleadoEnPantalla()
        {
            int id = Int32.Parse(txtID.Text);
            string script = "SELECT * FROM dbo.Ubicacion;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreUbicacion"];
                txtDescripcion.Text = (string)dt.Rows[id]["descripcionUbicacion"];
                BD.EnseniarBoton(btnCambiar);
                BD.NoEnseniarBoton(btnGrabar);
            }
           
            else
            {
                txtID.Text = "0";
                BD.EnseniarBoton(btnGrabar);
                BD.NoEnseniarBoton(btnCambiar);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idUbicacion", "dbo.Ubicacion", txtID);
            txtNombre.Clear();
            txtDescripcion.Clear();
            BD.EnseniarBoton(btnGrabar);
            BD.NoEnseniarBoton(btnCambiar);
        }
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            string script = "UPDATE [dbo].[Ubicacion]" +
                " SET [nombreUbicacion] = @nombreUbicacion " +
                ",[descripcionUbicacion] = @descripcionUbicacion" +
                "WHERE idUbicacion = @idUbicacion;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@nombreUbicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@descripcionUbicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtDescripcion.Text.ToString()));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);

        }
       
    }
}
