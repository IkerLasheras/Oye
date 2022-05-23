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
    public partial class WinProveedores : Window
    {

        public WinProveedores()
        {
            
            InitializeComponent();
            if (txtID.Text == "")
            {
                txtID.Text = "0";
            }
            EnseniarEnPantalla(Int32.Parse(txtID.Text));
            EnseniarComboBox(obtenerDataComboBox("dbo.Categorias", "nombreCategoria"),cbCategoria);
        }

        bool insertoImagen = false;
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BD.IncrementarID(txtID);
                EnseniarEnPantalla(Int32.Parse(txtID.Text));
              
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
        
        public void InsertarDatos()
        {
            string script =
                "INSERT INTO [dbo].[Proveedores] " +
                "(" +
                "[idProveedor]," +
                "[nombreProveedor]," +
                "[localizacionProveedor]," +
                "[iban]," +
                "[direccion]," +
                "[email]" +
                ",[categoriaProveedor]," +
                "[telefono])" +
                "VALUES" +
                "(" +
                "@idProveedor" +
                ", @nombreProveedor" +
                ", @localizacionProveedor" +
                ", @iban" +
                ", @direccion" +
                ", @email" +
                ", @categoriaProveedor" +
                ", @telefono);";
               

            string scriptImagen = "INSERT INTO [dbo].[Proveedores] ([imgProveedor]) VALUES (@imgProveedor) ;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idProveedor", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreProveedor", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@localizacionProveedor", SqlDbType.NChar, ParameterDirection.Input, false, txtLocalidad.Text.ToString()));
            listaParametros.Add(BD.ObtenerParametro("@iban", SqlDbType.NChar, ParameterDirection.Input, false, txtIBAN.Text));
            listaParametros.Add(BD.ObtenerParametro("@direccion", SqlDbType.NChar, ParameterDirection.Input, false,txtDireccion.Text));
            listaParametros.Add(BD.ObtenerParametro("@email", SqlDbType.NChar, ParameterDirection.Input, false, txtEmail.Text));
            listaParametros.Add(BD.ObtenerParametro("@categoriaProveedor", SqlDbType.Int, ParameterDirection.Input, false, cbCategoria.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@telefono", SqlDbType.NChar, ParameterDirection.Input, false, txtTelefono.Text));

            listaParametrosImg.Add(BD.ObtenerParametro("@imgProveedor", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
            listaParametrosImg.Add(BD.ObtenerParametro("@idProveedor", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);

            if (insertoImagen)
            {

                BD.LanzarComandoSQLNonQuery(scriptImagen, listaParametrosImg);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatos();
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
            insertoImagen = true;
            BD.InsertarImagen(imgProveedor,lblUrl,insertoImagen);
        }

        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {

            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgProveedor"]))
            {
                var result = dt.Rows[id]["imgProveedor"];
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

        private void EnseniarEnPantalla(int id)
        {

            txtID.Text = id.ToString();
            string script = "SELECT * FROM dbo.Proveedores;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreProveedor"];
                txtLocalidad.Text = (string)dt.Rows[id]["localizacionProveedor"];
                txtDireccion.Text = dt.Rows[id]["direccion"].ToString();
                txtTelefono.Text = (string)dt.Rows[id]["telefono"];
                txtIBAN.Text = dt.Rows[id]["iban"].ToString();
                txtEmail.Text = (string)dt.Rows[id]["email"];
                cbCategoria.SelectedIndex = (int)dt.Rows[id]["categoriaProveedor"];
                
                imgProveedor.Source = ObtenerImgBBDD(dt, id);

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
            BD.BuscarIDMax("idProveedor", "Proveedores" , txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

        private void vaciarCeldas()
        {
            TextBox[] textBoxes = { txtNombre, txtDireccion, txtEmail, txtIBAN, txtLocalidad, txtTelefono};
            ComboBox[] comboBoxes = { cbCategoria};
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            imgProveedor.Source = null;

        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.DisminuirID(txtID);
                EnseniarEnPantalla(Int32.Parse(txtID.Text));

            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }

        private void Modificar()
        {
            string script = "UPDATE [dbo].[Proveedores]" +
                " SET [nombreProveedor] = @nombreProveedor " +
                ",[localizacionProveedor] = @localizacionProveedor " +
                ",[iban] = @iban " +
                ",[categoriaProveedor] = @categoriaProveedor " +
                ",[direccion] = @direccion " +
                ",[telefono] = @telefono " +
                ",[email] = @email " +
                "WHERE idProveedor = @idProveedor;";

            string scriptImg = "UPDATE [dbo].[Proveedores] SET [imgProveedor] = @imgProveedor WHERE [idProveedor] = @idProveedor";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();


            listaParametros.Add(BD.ObtenerParametro("@idProveedor", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreProveedor", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@localizacionProveedor", SqlDbType.NChar, ParameterDirection.Input, false, txtLocalidad.Text.ToString()));
            listaParametros.Add(BD.ObtenerParametro("@iban", SqlDbType.NChar, ParameterDirection.Input, false, txtIBAN.Text));
            listaParametros.Add(BD.ObtenerParametro("@direccion", SqlDbType.NChar, ParameterDirection.Input, false, txtDireccion.Text));
            listaParametros.Add(BD.ObtenerParametro("@email", SqlDbType.NChar, ParameterDirection.Input, false, txtEmail.Text));
            listaParametros.Add(BD.ObtenerParametro("@categoriaProveedor", SqlDbType.Int, ParameterDirection.Input, false, cbCategoria.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@telefono", SqlDbType.NChar, ParameterDirection.Input, false, txtTelefono.Text));

            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@imgProveedor", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                listaParametrosImg.Add(BD.ObtenerParametro("@idProveedor", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
            }
            insertoImagen = false;

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            Modificar();
            insertoImagen = false;
        }

        private void btnCrearTipo_Click(object sender, RoutedEventArgs e)
        {
            Window crearTipoEmpleado = new CrearTipoEmpleado();
            crearTipoEmpleado.ShowDialog();
        }

        private string[] obtenerDataComboBox(string tabla ,string columnaNombre)
        {
            string script = "select " + columnaNombre + " from " + tabla + ";";
            string scriptCantidadTipo = "select count(*) as cantidad from "+tabla;

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

        }

        //private string[] TiposEmpleado()
        //{
        //    string script = "select nombreTipoEmpleado from tipoEmpleado";
        //    string scriptCantidadTipo = "select count(*) as cantidad from TipoEmpleado";

        //    DataTable dt2 = new DataTable();
        //    dt2 = BD.RellenarDataTable(dt2, scriptCantidadTipo);
        //    int cantidadTipos = (int)dt2.Rows[0]["cantidad"];

        //    string[] tipos = new string[cantidadTipos];

        //    DataTable dt = new DataTable();
        //    dt = BD.RellenarDataTable(dt, script);
            
        //    for(int i = 0; i<cantidadTipos; i++)
        //    {
        //        tipos[i] = (string)dt.Rows[i]["nombreTipoEmpleado"];
        //    }
        //    return tipos;
        //}
        private void EnseniarComboBox(string[] tipos, ComboBox cb)
        {
            for (int i = 0; i < tipos.Length; i++)
            {
               cb.Items.Add(tipos[i]);
            }
        }

        private void btnCrearCategoria_Click(object sender, RoutedEventArgs e)
        {
            Window categoria = new WinCategoriaProducto();
            categoria.ShowDialog();
        }

        private void btnCrearUbicacion_Click(object sender, RoutedEventArgs e)
        {
            Window ubicacion = new WinUbicacionProducto();
            ubicacion.ShowDialog();
        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window listado = new Proveedores.ListaProveedores();
            listado.ShowDialog();
            EnseniarEnPantalla(Proveedores.ListaProveedores.id);
        }
    }
}
