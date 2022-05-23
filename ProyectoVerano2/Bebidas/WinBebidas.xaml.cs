using Microsoft.Win32;
using Proyecto;
using ProyectoVerano2.Bebidas;
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
    public partial class WinBebidas : Window
    {

        public WinBebidas()
        {
            
            InitializeComponent();
            if (txtID.Text == "")
            {
                txtID.Text = "0";
            }
            EnseniarEnPantalla(Int32.Parse(txtID.Text));
            EnseniarComboBox(obtenerDataComboBox("dbo.TipoBebida", "nombre"), cbTipo);
            ;


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
                "INSERT INTO [dbo].[Bebidas] " +
                "(" +
                "[idBebida]," +
                "[precio]," +
                "[tipoBebida]," +
                "[nombreBebida])"+
                "VALUES" +
                "(" +
                "@idBebida" +
                ", @precio" +
                ", @tipoBebida" +
                ", @nombreBebida);";
               

            string scriptImagen = "INSERT INTO [dbo].[Bebidas] ([imgBebida]) VALUES (@imgBebida) ;";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@precio", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtPrecio.Text)));
            listaParametros.Add(BD.ObtenerParametro("@tipoBebida", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));


            listaParametrosImg.Add(BD.ObtenerParametro("@imgBebida", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
            listaParametrosImg.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));

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
            BD.InsertarImagen(imgBebida,lblUrl,insertoImagen);
        }

        private BitmapImage ObtenerImgBBDD(DataTable dt, int id)
        {

            ImageClass images = new ImageClass();
            if (!Convert.IsDBNull(dt.Rows[id]["imgBebida"]))
            {
                var result = dt.Rows[id]["imgBebida"];
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
            string script = "SELECT * FROM dbo.Bebidas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = (string)dt.Rows[id]["nombreBebida"];
                txtPrecio.Text = dt.Rows[id]["precio"].ToString();
                cbTipo.SelectedIndex = (int)dt.Rows[id]["tipoBebida"];
                
                imgBebida.Source = ObtenerImgBBDD(dt, id);

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
            BD.BuscarIDMax("idBebida", "Bebidas" , txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

        private void vaciarCeldas()
        {
            TextBox[] textBoxes = { txtNombre, txtPrecio};
            ComboBox[] comboBoxes = { cbTipo };
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = 0;
            }

            imgBebida.Source = null;

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
            string script = "UPDATE [dbo].[Bebidas]" +
                " SET [nombreBebida] = @nombreBebida " +
                ",[precio] = @precio " +
                ",[tipoBebida] = @tipoBebida; ";

            string scriptImg = "UPDATE [dbo].[Bebidas] SET [imgBebida] = @imgBebida WHERE [idBebida] = @idBebida";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            List<SqlParameter> listaParametrosImg = new List<SqlParameter>();


            listaParametros.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@nombreBebida", SqlDbType.NChar, ParameterDirection.Input, false, txtNombre.Text));
            listaParametros.Add(BD.ObtenerParametro("@precio", SqlDbType.Float, ParameterDirection.Input, false, float.Parse(txtPrecio.Text)));
            listaParametros.Add(BD.ObtenerParametro("@tipoBebida", SqlDbType.Int, ParameterDirection.Input, false, cbTipo.SelectedIndex));

            if (insertoImagen)
            {
                listaParametrosImg.Add(BD.ObtenerParametro("@imgBebida", SqlDbType.VarBinary, ParameterDirection.Input, true, CambiarFotoParaBBDD().imagen));
                listaParametrosImg.Add(BD.ObtenerParametro("@idBebida", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
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
            Window crearTipoBebida = new WinTipoBebida();
            crearTipoBebida.ShowDialog();
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

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window lista = new Bebidas.ListaBebidas();
            lista.ShowDialog();
            EnseniarEnPantalla(ListaBebidas.id); 
        }
    }
}
