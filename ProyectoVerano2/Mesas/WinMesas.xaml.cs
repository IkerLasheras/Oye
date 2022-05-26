using Microsoft.Win32;
using Proyecto;
using ProyectoVerano2.Bebidas;
using ProyectoVerano2.Mesas;
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
    public partial class WinMesas : Window
    {

        public WinMesas()
        {
            
            InitializeComponent();
            if (txtID.Text == "")
            {
                txtID.Text = "0";
            }
            EnseniarEnPantalla(Int32.Parse(txtID.Text));
           
            ;


        }

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
            int checkBox = 0;
            if ((bool)chbDisponible.IsChecked)
            {
                checkBox = 1;
            }
            else
            {
                checkBox |= 2;
            }

            string script =
                "INSERT INTO [dbo].[Mesas] " +
                "(" +
                "[idMesa]," +
                "[ubicacion]," +
                "[disponible])"+
                "VALUES" +
                "(" +
                "@idMesa" +
                ", @ubicacion" +
                ", @disponible);";

            List<SqlParameter> listaParametros = new List<SqlParameter>();

            listaParametros.Add(BD.ObtenerParametro("@idMesa", SqlDbType.Int, ParameterDirection.Input, true, Int32.Parse(txtID.Text)));
            listaParametros.Add(BD.ObtenerParametro("@ubicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtUbicacion.Text));
            listaParametros.Add(BD.ObtenerParametro("@disponible", SqlDbType.Bit, ParameterDirection.Input, false, checkBox));
            
            BD.LanzarComandoSQLNonQuery(script, listaParametros);

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            InsertarDatos();
           

            BD.NoEnseniarBoton(btnGuardar);
            BD.EnseniarBoton(btnCambiar);
        }

      
        private void EnseniarEnPantalla(int id)
        {
            txtID.Text = id.ToString();
            string script = "SELECT * FROM dbo.Mesas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            if (dt.Rows.Count > 0)
            {
                txtUbicacion.Text = (string)dt.Rows[id]["ubicacion"];
                if ((bool)dt.Rows[id]["disponible"])
                {
                    chbDisponible.IsChecked = true;
                }
                else
                {
                    chbDisponible.IsChecked = false;
                }

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
            BD.BuscarIDMax("idMesa", "Mesas" , txtID);
            vaciarCeldas();

            btnGuardar.IsEnabled = true;
            btnGuardar.Visibility = Visibility.Visible;
            btnCambiar.IsEnabled = false;
            btnCambiar.Visibility = Visibility.Collapsed;
        }

        private void vaciarCeldas()
        {
            TextBox[] textBoxes = { txtUbicacion};
           
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
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
            int checkBox = 0;
            if ((bool)chbDisponible.IsChecked)
            {
                checkBox = 1;
            }
            else
            {
                checkBox |= 2;
            }
            string script = "UPDATE [dbo].[Mesas]" +
                " SET [ubicacion] = @ubicacion " +
                ",[disponible] = @disponible ";

            List<SqlParameter> listaParametros = new List<SqlParameter>();
            
            listaParametros.Add(BD.ObtenerParametro("@ubicacion", SqlDbType.NChar, ParameterDirection.Input, false, txtUbicacion.Text));
            listaParametros.Add(BD.ObtenerParametro("@disponible", SqlDbType.Float, ParameterDirection.Input, false, checkBox));

            BD.LanzarComandoSQLNonQuery(script, listaParametros);
        }

        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            Modificar();

        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            Window lista = new Mesas.ListaMesas();
            lista.ShowDialog();
            EnseniarEnPantalla(ListaMesas.id); 
        }
    }
}
