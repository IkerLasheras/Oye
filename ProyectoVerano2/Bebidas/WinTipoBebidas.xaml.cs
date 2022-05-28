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
    public partial class WinTipoBebida : Window
    {
        bool insertoImagen = false;
        public WinTipoBebida()
        {
            InitializeComponent();
            txtID.Text = "0";
            EnseniarTipoBebidaEnPantalla();

        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarTipoBebidaEnPantalla();
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
            string script = "INSERT INTO [dbo].[TipoBebida] ([idTipoBebida],[nombre]) VALUES (@idTipoBebida, @nombreBebida);";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idTipoBebida", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }
    
    private void EnseniarTipoBebidaEnPantalla()
        {
            int id = Int32.Parse(txtID.Text);
            string script = "SELECT * FROM dbo.TipoBebida;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombre"];
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
                EnseniarTipoBebidaEnPantalla();
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idTipoBebida", "dbo.TipoBebida", txtID);
            txtNombre.Clear();
            BD.EnseniarBoton(btnGrabar);
            BD.NoEnseniarBoton(btnCambiar);
        }
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            string script = "UPDATE [dbo].[TipoBebida]" +
                " SET [nombre] = @nombre " +
                "WHERE idTipoBebida = @idTipoBebida;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@nombre", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@idTipoBebida", SqlDbType.Int, ParameterDirection.Input, false, Int32.Parse(txtID.Text)));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);

        }
       
    }
}
