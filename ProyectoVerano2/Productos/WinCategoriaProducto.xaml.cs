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
    public partial class WinCategoriaProducto : Window
    {
        bool insertoImagen = false;
        public WinCategoriaProducto()
        {
            InitializeComponent();
            txtID.Text = "0";
            EnseniarCategoriaEnPantalla();

        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BD.IncrementarID(txtID);
                EnseniarCategoriaEnPantalla();
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
            string script= "INSERT INTO [dbo].[Categorias] ([idCategoria],[descripcionCategoria],[nombrecategoria]) VALUES (@idCategoria, @descripcionCategoria, @nombreCategoria);";
            string scriptImg = "Update [dbo].[Categorias]" +
               "set[imgCategoria] = @imgCategoria " +
               "where [idCategoria] = @idImgCategoria;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idCategoria", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametros.Add(BD.ObtenerParametro("@descripcionCategoria", SqlDbType.NChar, ParameterDirection.Input, false, txtDescripcion.Text));
            listaParametros.Add(BD.ObtenerParametro("@nombreCategoria", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));


            listaParametrosImg.Add(BD.ObtenerParametro("@idImgCategoria", SqlDbType.Int, ParameterDirection.Input, true, txtID.Text));
            listaParametrosImg.Add(BD.ObtenerParametro("@imgCategoria", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));


            BD.LanzarComandoSQLNonQuery(script, listaParametros);

            if (insertoImagen)
            {
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
            }
        }

    
    private void EnseniarCategoriaEnPantalla()
        {
            int id = Int32.Parse(txtID.Text);
            string script = "SELECT * FROM dbo.Categorias;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreCategoria"];
                txtDescripcion.Text = (string)dt.Rows[id]["descripcionCategoria"];
                imgCategoriaProducto.Source = ObtenerImgBBDD(dt, id);
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
                EnseniarCategoriaEnPantalla();
            }
            catch (Exception ex)
            {
                txtID.Text = "0";
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            BD.BuscarIDMax("idCategoria" , "Categorias", txtID);
            txtNombre.Clear();
            txtDescripcion.Clear();
            imgCategoriaProducto.Source = null;
            BD.EnseniarBoton(btnGrabar);
            BD.NoEnseniarBoton(btnCambiar);
        }
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            string script = "UPDATE [dbo].[Categorias]" +
                " SET [nombreCategoria] = @nombreCategoria " +
                ",[descripcionCategoria] = @descripcionCategoria " +
                "WHERE idCategoria = @idCategoria;";

            string scriptImg = "Update [dbo].[Categorias]" +
                "set[imgCategoria] = @imgCategoria " +
                "where [idCategoria] = @idImgCategoria;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idCategoria", SqlDbType.Int, ParameterDirection.Input, false, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreCategoria", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@descripcionCategoria", SqlDbType.NChar, ParameterDirection.Input, false, txtDescripcion.Text));

            listaParametrosImg.Add(BD.ObtenerParametro("@imgCategoria", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
            listaParametrosImg.Add(BD.ObtenerParametro("@idImgCategoria", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);

            if (insertoImagen)
            {
                BD.LanzarComandoSQLNonQuery(scriptImg, listaParametrosImg);
            }
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
        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {

            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgCategoria"]))
            {
                var result = dt.Rows[id]["imgCategoria"];
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

        private void btnInsertarImg_Click(object sender, RoutedEventArgs e)
        {
            insertoImagen = true;
            BD.InsertarImagen(imgCategoriaProducto, lblUrl, insertoImagen);

        }

       
    }
}
