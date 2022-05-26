using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Controls;
using System.Configuration;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Proyecto
{
    public class BD
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["PruebaCasa"].ConnectionString;
          
        public static SqlConnection CrearConexion()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            return connection;
        }

        public static void AbrirConexion(SqlConnection connection, SqlCommand command)
        {
            command.Connection.Open();
        }
        public static void CerrarConexion(SqlConnection connection)
        {
            connection.Close();
        }
        public static SqlCommand CrearComando(SqlConnection connection, string script)
        {
            return new SqlCommand(script, connection);
        }
        public static SqlDataAdapter CrearDataAdapter(SqlCommand command)
        {
            return new SqlDataAdapter(command);
        }

        public static DataTable CrearDataTable(SqlDataAdapter da)
        {
            DataTable dt = new DataTable();
            return dt;
        }

        public static DataTable RellenarDataTable(DataTable dt, string script)
        {
            SqlConnection connection = CrearConexion();
            SqlCommand command = CrearComando(connection, script);

            SqlDataAdapter da = CrearDataAdapter(command);
            dt = CrearDataTable(da);

            AbrirConexion(connection, command);
            try
            {
                da.Fill(dt);
            }catch(Exception ex)
            {

            }
        

            return dt;
        }

        public static SqlParameter ObtenerParametro(string stringParametro, SqlDbType tipoParametro, ParameterDirection direccion, bool valorNulo, Object valor)
        {
            SqlParameter parametro = new SqlParameter(stringParametro, tipoParametro);
            parametro.IsNullable = valorNulo;
            parametro.Direction = direccion;
            parametro.Value = valor;
            return parametro;
        }

        public static void LanzarComandoSQLNonQuery(string cadenaSQL,  List<SqlParameter> listaParametros)
        {
            try
            {
                SqlConnection conexion = CrearConexion();
            SqlCommand comando = CrearComando(conexion, cadenaSQL);
            AbrirConexion(conexion, comando);
            AniadirParametroComandos(comando, listaParametros);
            EjecutarQuery(comando);

            CerrarConexion(conexion);
        }
            catch (Exception e)
            {
              
            }
           
        }
        public static void AniadirParametroComandos(SqlCommand comando, List<SqlParameter> listaParametros)
        {
            for (int i = 0; i < listaParametros.Count; i++)
            {
                comando.Parameters.Add(listaParametros[i]);
            }
        }
        /// <returns></returns>
        public static int EjecutarQuery(SqlCommand comando)
        {
            int numeroFilas = 0;

            try
            {
                numeroFilas = comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error ejecutando el Query");
            }
            return numeroFilas;
        }

        public static void IncrementarID(TextBox txt)
        {
            txt.Text = (Int32.Parse(txt.Text) + 1).ToString();
        }

        public static void DisminuirID(TextBox txt)
        {
            txt.Text = (Int32.Parse(txt.Text) -1).ToString();
        }

        public static void BuscarIDMax(string columna, string tabla , TextBox txt)
        {
            int id = 0;
            string script = "SELECT max(" + columna + ") FROM  " + tabla + ";";

            DataTable dt = new DataTable();
            dt = RellenarDataTable(dt,script);
            id = (int)dt.Rows[0][0] + 1;
            txt.Text = id.ToString();
        }

        public static int BuscarUltimoPedido()
        {
            int id = 0;
            string script = "SELECT max(idPedidos) FROM  PedidoMesa;";

            DataTable dt = new DataTable();
            dt = RellenarDataTable(dt, script);
            try
            {
                id = Int32.Parse((string)dt.Rows[0][0]) + 1;
            }catch (Exception ex)
            {
                id = 0;
            }
            

            return id;
        }

        public static void EnseniarBoton(Button boton)
        {
            boton.IsEnabled = true;
            boton.Visibility = Visibility.Visible;
        }

        public static void NoEnseniarBoton(Button boton)
        {
            boton.IsEnabled = false;
            boton.Visibility = Visibility.Collapsed;
        }

        public static async void InsertarImagen(Image img , Label lbl , bool insertoImagen)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Archivos de imágenes (*.bmp, *.jpg)|*.bmp;*.jpg|Todos los archivos (*.*)|*.*";

            openFileDialog1.DefaultExt = ".jpeg";

            openFileDialog1.ShowDialog();

            lbl.Content = openFileDialog1.FileName;

            ImageSource imageSource = new BitmapImage(new Uri((string)lbl.Content));

            img.Source = imageSource;

            insertoImagen = true;

        }
    }

}

