using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Proyecto
{
    public class BD
    {
        public static SqlConnection CrearConexion(string connectionString)
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

        public static DataTable RellenarDataTable(DataTable dt, string script, string connectionString)
        {
            SqlConnection connection = CrearConexion(connectionString);
            SqlCommand command = CrearComando(connection, script);

            SqlDataAdapter da = CrearDataAdapter(command);
            dt = CrearDataTable(da);

            AbrirConexion(connection, command);

            da.Fill(dt);

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

        public static void LanzarComandoSQLNonQuery(string cadenaSQL, string cadenaConexion, List<SqlParameter> listaParametros)
        {
            //try
            //{
            SqlConnection conexion = CrearConexion(cadenaConexion);
            SqlCommand comando = CrearComando(conexion, cadenaSQL);
            AbrirConexion(conexion, comando);
            AniadirParametroComandos(comando, listaParametros);
            EjecutarQuery(comando);

            CerrarConexion(conexion);
            //}
            //catch (Exception e)
            //{
            //  return e;
            // }
            //return null;
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

            //try
            //{
            numeroFilas = comando.ExecuteNonQuery();
            //}
            //catch (Exception e)
            //{
            // Debug.WriteLine("Error ejecutando el Query");
            //}
            return numeroFilas;
        }
    }

}

