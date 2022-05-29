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
    public partial class WinProductos : Window
    {

        public WinProductos()
        {
            
            InitializeComponent();
            if (txtID.Text == "")
            {
                txtID.Text = "0";
            }
           
            EnseniarEnPantalla(Int32.Parse(txtID.Text));
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.Categorias", "nombreCategoria"), cbCategoria);
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.Ubicacion", "nombreUbicacion"), cbUbicacion);
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
                "INSERT INTO [dbo].[Productos] " +
                "(" +
                "[idProducto]," +
                "[nombreProducto]," +
                "[codigoProducto]," +
                "[cantidadStock]," +
                "[unidadMedida]," +
                "[costeUnitario]" +
                ",[categoriaProducto]," +
                "[ubicacion])" +
                "VALUES" +
                "(" +
                "@idProducto" +
                ", @nombreProducto" +
                ", @codigoProducto" +
                ", @cantidadStock" +
                ", @unidadMedida" +
                ", @costeUnitario" +
                ", @categoriaProducto" +
                ", @ubicacion);";  

            string scriptImagen = "UPDATE [dbo].[Productos] SET [imgProducto] = @imgProducto WHERE [idProducto] = @idProducto";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idProducto", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreProducto", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@codigoProducto", SqlDbType.NChar, ParameterDirection.Input, false, txtCodigo.Text.ToString()));
            listaParametros.Add(BD.ObtenerParametro("@cantidadStock", SqlDbType.Int, ParameterDirection.Input, false, Int32.Parse(txtCantidadStock.Text)));
            listaParametros.Add(BD.ObtenerParametro("@unidadMedida", SqlDbType.NChar, ParameterDirection.Input, false, txtUMedida.Text.ToString()));
            listaParametros.Add(BD.ObtenerParametro("@costeUnitario", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtCosteUnitario.Text)));
            listaParametros.Add(BD.ObtenerParametro("@categoriaProducto", SqlDbType.Int, ParameterDirection.Input, false, cbCategoria.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@ubicacion", SqlDbType.Int, ParameterDirection.Input, false, cbUbicacion.SelectedIndex));

            listaParametrosImg.Add(BD.ObtenerParametro("@imgProducto", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
            listaParametrosImg.Add(BD.ObtenerParametro("@idProducto", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));

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
            BD.InsertarImagen(imgProducto,lblUrl,insertoImagen);
        }

        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {

            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgProducto"]))
            {
                var result = dt.Rows[id]["imgProducto"];
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
            string script = "SELECT * FROM dbo.Productos;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreProducto"];
                txtCodigo.Text = (string)dt.Rows[id]["codigoProducto"];
                txtCantidadStock.Text = dt.Rows[id]["cantidadStock"].ToString();
                txtUMedida.Text = (string)dt.Rows[id]["unidadMedida"];
                txtCosteUnitario.Text = dt.Rows[id]["costeUnitario"].ToString();
                cbCategoria.SelectedIndex = (int)dt.Rows[id]["categoriaProducto"];
                cbUbicacion.SelectedIndex = (int)dt.Rows[id]["ubicacion"];
                
                imgProducto.Source = ObtenerImgBBDD(dt, id);

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
            BD.BuscarIDMax("idProducto", "Productos" , txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

        private void vaciarCeldas()
        {
            TextBox[] textBoxes = { txtNombre, txtCantidadStock, txtCodigo, txtCosteUnitario, txtUMedida};
            ComboBox[] comboBoxes = { cbCategoria, cbUbicacion };
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            imgProducto.Source = null;

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
            string script = "UPDATE [dbo].[Productos]" +
                " SET [nombreProducto] = @nombreProducto " +
                ",[codigoProducto = @codigoProducto " +
                ",[cantidadStock] = @cantidadStock " +
                ",[categoriaProducto] = @categoriaProducto " +
                ",[costeUnitario] = @costeUnitario " +
                ",[unidadMedida] = @unidadMedida " +
                ",[ubicacion] = @ubicacion " +
                "WHERE idProducto = @idProducto;";

            string scriptImg = "UPDATE [dbo].[Productos] SET [imgProducto] = @imgProducto WHERE [idProducto] = @idProducto";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();


            listaParametros.Add(BD.ObtenerParametro("@idProducto", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreProducto", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@codigoProducto", SqlDbType.NChar, ParameterDirection.Input, false, txtCodigo.Text));
            listaParametros.Add(BD.ObtenerParametro("@cantidad", SqlDbType.Int, ParameterDirection.Input, false, Int32.Parse(txtCantidadStock.Text)));
            listaParametros.Add(BD.ObtenerParametro("@unidadMedida", SqlDbType.NChar, ParameterDirection.Input, false, txtUMedida.Text));
            listaParametros.Add(BD.ObtenerParametro("@costeUnitario", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtCosteUnitario.Text)));
            listaParametros.Add(BD.ObtenerParametro("@categoriaProducto", SqlDbType.Int, ParameterDirection.Input, false, cbCategoria.SelectedItem));
            listaParametros.Add(BD.ObtenerParametro("@ubicacion", SqlDbType.Int, ParameterDirection.Input, false, cbUbicacion.SelectedIndex));

            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@imgProducto", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                listaParametrosImg.Add(BD.ObtenerParametro("@idProducto", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
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

        private void btnCrearCategoria_Click(object sender, RoutedEventArgs e)
        {
            Window categoria = new WinCategoriaProducto();
            categoria.ShowDialog();
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.Categorias", "nombreCategoria"), cbCategoria);
          
        }

        private void btnCrearUbicacion_Click(object sender, RoutedEventArgs e)
        {
            Window ubicacion = new WinUbicacionProducto();
            ubicacion.ShowDialog();
            BD.EnseniarComboBox(BD.ObtenerDataComboBox("dbo.Ubicacion", "nombreUbicacion"), cbUbicacion);
        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window listado = new Productos.ListaProductos();
            listado.ShowDialog();
            EnseniarEnPantalla(Productos.ListaProductos.id);
        }
    }
}
