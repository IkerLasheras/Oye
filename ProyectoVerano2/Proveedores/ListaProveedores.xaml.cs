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

namespace ProyectoVerano2.Proveedores
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ListaProveedores: Window
    {


        // public static string campoBusqueda = cbCampoBusqueda.SelectedItem.ToString();


        public static int id = 0;
        public static string scriptBusquedaProveedor = "SELECT * FROM dbo.Proveedores;";

        public ListaProveedores()
        {
            InitializeComponent();
            EnseniarPlatos();
        }

        private void EnseniarPlatos()
        {
            ObtenerCatalogoProveedores obtenerCatalogoProveedores= new ObtenerCatalogoProveedores();
            List<Proveedores> lista = obtenerCatalogoProveedores.GetProveedoresActives();

            this.dtgProveedores.ItemsSource = lista;
        }

        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgProveedores.SelectedIndex != -1)
            {
                Proveedores? Proveedores = this.dtgProveedores.SelectedItem as Proveedores;

                id = Proveedores.ID;
                Close();
                
            }
        }



        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbNombre)
            {
                scriptBusquedaProveedor = "SELECT * FROM dbo.Proveedor where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else if(cbCampoBusqueda.SelectedItem == cbTipo)
            {
                scriptBusquedaProveedor = "select * from Proveedor where categoriaProveedor = ( select idCategoria from Categorias where nombreCategoria like '" + txtValor.Text +"%'); ";
            }
            else {
                scriptBusquedaProveedor = "SELECT * FROM dbo.Proveedor where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
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
