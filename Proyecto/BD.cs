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
        static string connectionString = ConfigurationManager.ConnectionStrings["Proyecto"].ConnectionString;
          
        /// <summary>
        /// Genera una conexión.
        /// </summary>
        /// <returns> Nueva conexión</returns>
        public static SqlConnection CrearConexion()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            return connection;
        }

        /// <summary>
        /// Abrir conexion desde una conexion y un comando ya exisentes
        /// </summary>
        /// <param name="connection">Conexion que quieres abrir</param>
        /// <param name="command">Comando que queres realizar</param>
        public static void AbrirConexion(SqlConnection connection, SqlCommand command)
        {
            command.Connection.Open();
        }

        /// <summary>
        /// Cierra una conexión dada
        /// </summary>
        /// <param name="connection">Conexion a cerrar</param>
        public static void CerrarConexion(SqlConnection connection)
        {
            connection.Close();
        }

        /// <summary>
        /// Generar un commando desde una conexion y una cadena con un script
        /// </summary>
        /// <param name="connection">Conexion desde la que se quiere trabajar</param>
        /// <param name="script">Script a ejecutar</param>
        /// <returns></returns>
        public static SqlCommand CrearComando(SqlConnection connection, string script)
        {
            return new SqlCommand(script, connection);
        }

        /// <summary>
        /// Crear Adapter desde un comando
        /// </summary>
        /// <param name="command">Comando a realizar</param>
        /// <returns></returns>
        public static SqlDataAdapter CrearDataAdapter(SqlCommand command)
        {
            return new SqlDataAdapter(command);
        }

        /// <summary>
        /// Crear una tabla de datos.
        /// </summary>
        /// <returns>Una nueva tabla vacia</returns>
        public static DataTable CrearDataTable()
        {
            DataTable dt = new DataTable();
            return dt;
        }

        /// <summary>
        /// Rellena la datatable usando el dataAdapter
        /// </summary>
        /// <param name="dt">Datatable a rellenar</param>
        /// <param name="script">Script de consulta con lo que quieres rellenar</param>
        /// <returns>DataTable rellenada</returns>
        public static DataTable RellenarDataTable(DataTable dt, string script)
        {
            SqlConnection connection = CrearConexion();
            SqlCommand command = CrearComando(connection, script);

            SqlDataAdapter da = CrearDataAdapter(command);
            dt = CrearDataTable();

            AbrirConexion(connection, command);
            try
            {
                da.Fill(dt);
            }catch(Exception ex)
            {
            }
            return dt;
        }

        /// <summary>
        /// Genera un parámetro para despues poder utilizarlo.
        /// </summary>
        /// <param name="stringParametro">nombre del parámetro</param>
        /// <param name="tipoParametro">Tipo de dato del parametro</param>
        /// <param name="direccion">Dirección de entrada o salida</param>
        /// <param name="valorNulo">Booleano si puede ser nulo o no</param>
        /// <param name="valor"> valor a insertar</param>
        /// <returns></returns>
        public static SqlParameter ObtenerParametro(string stringParametro, SqlDbType tipoParametro, ParameterDirection direccion, bool valorNulo, Object valor)
        {
            SqlParameter parametro = new SqlParameter(stringParametro, tipoParametro);
            parametro.IsNullable = valorNulo;
            parametro.Direction = direccion;
            parametro.Value = valor;
            return parametro;
        }

        /// <summary>
        /// Crea todo lo necesario y ejecuta el Query
        /// </summary>
        /// <param name="cadenaSQL">Cadena de Script de Busqueda </param>
        /// <param name="listaParametros">Lista de parametrods necesarios para el query.</param>
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

        /// <summary>
        /// Añade todos los parametros de la lista en el comando dado
        /// </summary>
        /// <param name="comando">Comando al que se quiere insertar los parametros</param>
        /// <param name="listaParametros">Parametros a insertar</param>
        public static void AniadirParametroComandos(SqlCommand comando, List<SqlParameter> listaParametros)
        {
            for (int i = 0; i < listaParametros.Count; i++)
            {
                comando.Parameters.Add(listaParametros[i]);
            }
        }

        /// <summary>
        /// Ejecuta el query desde el comando otrogado
        /// </summary>
        /// <param name="comando">Comando desde el que se quiere ejecutar la query</param>
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

        /// <summary>
        /// Incrementa el número que hay en el TextBoxPasado
        /// </summary>
        /// <param name="txt">TextBox a incrementar</param>
        public static void IncrementarID(TextBox txt)
        {
            txt.Text = (Int32.Parse(txt.Text) + 1).ToString();
        }

        /// <summary>
        /// Disminuir el número que hay en el TextBoxPasado
        /// </summary>
        /// <param name="txt">TextBox a incrementar</param>
        public static void DisminuirID(TextBox txt)
        {
            txt.Text = (Int32.Parse(txt.Text) -1).ToString();
        }

        /// <summary>
        /// Busca el valor maximo de una columna y una tabla dada y lo coloca en un TextBox dado
        /// </summary>
        /// <param name="columna">Columna que se quiere conocer el máximo</param>
        /// <param name="tabla">Tabla donde se encuentra la columna dada</param>
        /// <param name="txt">TextBox a rellenar</param>
        public static void BuscarIDMax(string columna, string tabla , TextBox txt)
        {
            int id = 0;
            string script = "SELECT max(" + columna + ") FROM  " + tabla + ";";

            DataTable dt = new DataTable();
            dt = RellenarDataTable(dt,script);
            
            id = (int)dt.Rows[0][0] + 1;
            txt.Text = id.ToString();
        }

        /// <summary>
        /// Buscar el pedido Maximo 
        /// </summary>
        /// <returns>Id máximo</returns>
        public static int BuscarUltimoPedido()
        {
            int id = 0;
            string script = "SELECT max(idPedidos) FROM  Pedidos;";

            DataTable dt = new DataTable();
            dt = RellenarDataTable(dt, script);
            try
            {
                id = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }catch (Exception ex)
            {
                id = 0;
            }
            return id;
        }

        /// <summary>
        /// Muestra un boton habilitandolo y enseñandolo
        /// </summary>
        /// <param name="boton">Boton a enseñar</param>
        public static void EnseniarBoton(Button boton)
        {
            boton.IsEnabled = true;
            boton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Oculta un boton desabilitandolo y escondiendolo
        /// </summary>
        /// <param name="boton">Boton a ocultar</param>
        public static void NoEnseniarBoton(Button boton)
        {
            boton.IsEnabled = false;
            boton.Visibility = Visibility.Collapsed;
        }

        public static async void InsertarImagen(Image img , Label lbl , bool insertoImagen)
        {
            try
            {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Archivos de imágenes (*.bmp, *.jpg)|*.bmp;*.jpg|Todos los archivos (*.*)|*.*";

            openFileDialog1.DefaultExt = ".jpeg";

            openFileDialog1.ShowDialog();

            lbl.Content = openFileDialog1.FileName;
            
            ImageSource imageSource = new BitmapImage(new Uri((string)lbl.Content));

            img.Source = imageSource;

            insertoImagen = true;

            }catch(Exception ex)
            {
                insertoImagen = false;
            }
        }

        /// <summary>
        /// Rellena un array con los datos solicitados de una columna dada
        /// </summary>
        /// <param name="tabla">Tabla en la que esta la columna</param>
        /// <param name="columnaNombre">Columna a sacar datos</param>
        /// <returns></returns>
        public static  string[] ObtenerDataComboBox(string tabla, string columnaNombre)
        {
            try
            {
                string script = "select " + columnaNombre + " from " + tabla + ";";
                string scriptCantidadTipo = "select count(*) as cantidad from " + tabla;

                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, scriptCantidadTipo);
                int cantidadTipos = (int)dt2.Rows[0]["cantidad"];

                string[] tipos = new string[cantidadTipos];

                DataTable dt = new DataTable();
                dt = BD.RellenarDataTable(dt, script);

                for (int i = 0; i < cantidadTipos; i++)
                {
                    tipos[i] = (string)dt.Rows[i][columnaNombre];
                }
                return tipos;
            }catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Añade como items de combobox los datos de la array.
        /// </summary>
        /// <param name="tipos"></param>
        /// <param name="cb"></param>
        public static void EnseniarComboBox(string[] tipos, ComboBox cb)
        {
            cb.Items.Clear();
            for (int i = 0; i < tipos.Length; i++)
            {
                cb.Items.Add(tipos[i]);
            }
        }

    }

}

