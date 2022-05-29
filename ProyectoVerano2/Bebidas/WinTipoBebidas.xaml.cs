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
        /// <summary>
        /// Inicializa los componentes, Inserta un 0 en el TextBox del ID, Rellena los textBoxes con la información necesarios
        /// </summary>
        public WinTipoBebida()
        {
            InitializeComponent();
            txtID.Text = "0";
            EnseniarTipoBebidaEnPantalla();

        }

        /// <summary>
        /// Recoge los Datos de los TextBoxes y se insertan en la base de datos.
        /// </summary>
        private void InsertarEnBBDD()
        {
            string script = "INSERT INTO [dbo].[TipoBebida] ([idTipoBebida],[nombre]) VALUES (@idTipoBebida, @nombreBebida);";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idTipoBebida", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        /// <summary>
        /// Recoge los Datos de los TextBoxes y se  modifican en la base de datos con la condicion de el contenido del TextBox de ID
        /// </summary>
        private void ModificarEnBBDD()
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

        /// <summary>
        /// Rellena los TextBoxes con los datos de cada tipo de Bebida por ID
        /// </summary>
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

        /// <summary>
        /// Evento de click en el boton de grabar, realiza la insercción de los los datos mediante el metodo InsertarEnBBDD.
        /// Intercambia los botones Cambiar y Guardar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            InsertarEnBBDD();
            insertoImagen = false;
            BD.NoEnseniarBoton(btnGrabar);
            BD.EnseniarBoton(btnCambiar);
        }

        /// <summary>
        /// Evento de click en el boton de nuevo, realiza una consulta a la BBDD del maximo ID mediante el metodo BuscarIdmax
        /// , borra el contenido de los TextBoxes e intercambia el boton de grabar por el de cambiar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idTipoBebida", "dbo.TipoBebida", txtID);
            txtNombre.Clear();
            BD.EnseniarBoton(btnGrabar);
            BD.NoEnseniarBoton(btnCambiar);
        }

        /// <summary>
        /// Evento de click en el boton de cambiar, Realiza la modificacion de la pase de datos mediante el metodo de ModificarEnBBDD().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            ModificarEnBBDD();
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
        /// y rellena los textBoxes con la informaciónd e dicho Id con el metodo de EnseniarTipoBebidaEnPantalla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Evento de click en el boton de Anterior. Disminuye el id mediante el mediante el metodo DisminuirID()
        /// y rellena los textBoxes con la informaciónd e dicho Id con el metodo de EnseniarTipoBebidaEnPantalla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

    }
}
