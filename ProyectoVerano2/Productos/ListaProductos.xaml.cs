using ProyectoVerano2.Bebidas;
using System;
using System.Collections.Generic;
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

namespace ProyectoVerano2.Productos
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ListaProductos : Window
    {


        // public static string campoBusqueda = cbCampoBusqueda.SelectedItem.ToString();


        public static int id = 0;
        public static string scriptBusquedaProductos = "SELECT * FROM dbo.Productos;";
        public ListaProductos()
        {
            InitializeComponent();
            EnseniarPlatos();
        }

        private void EnseniarPlatos()
        {
            ObtenerCatalogoProductos obtenerCatalogoProductos = new ObtenerCatalogoProductos();
            List<Productos> lista = obtenerCatalogoProductos.GetProductosActives();

            this.dtgProductos.ItemsSource = lista;
        }

        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgProductos.SelectedIndex != -1)
            {
                Productos? productos = this.dtgProductos.SelectedItem as Productos;

                id = productos.ID;
                Close();
                
            }
        }



        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbNombre)
            {
                scriptBusquedaProductos = "SELECT * FROM dbo.Productos where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else if(cbCampoBusqueda.SelectedItem == cbTipo)
            {
                scriptBusquedaProductos = "select * from Productos where CategoriaProducto = ( select idCategoria from Categorias where nombreCategoria like '" + txtValor.Text +"%'); ";
            }
            else {
                scriptBusquedaProductos = "SELECT * FROM dbo.Productos where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
            }
            EnseniarPlatos();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtValor_KeyUp(object sender, KeyEventArgs e)
        {
            Buscar();
        }
    }
}
