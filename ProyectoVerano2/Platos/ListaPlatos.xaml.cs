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

namespace ProyectoVerano2.Platos
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ListaPlatos : Window
    {

        public static int id = 0;
        public static string scriptBusquedaPlatos = "SELECT * FROM dbo.Platos;";
        public ListaPlatos()
        {
            InitializeComponent();
            EnseniarPlatos();
        }

        private void EnseniarPlatos()
        {
            ObtenerCatalogoPlatos obtenerCatalogoPlatos = new ObtenerCatalogoPlatos();
            List<Platos> lista = obtenerCatalogoPlatos.GetPlatosActives();

            this.dtgPlatos.ItemsSource = lista;
        }

        private void dtgPlatos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgPlatos.SelectedIndex != -1)
            {
                Platos? Platos = this.dtgPlatos.SelectedItem as Platos;

                id = Platos.ID;
                Close();
                
            }
        }



        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbNombre)
            {
                scriptBusquedaPlatos = "SELECT * FROM dbo.Platos where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else if(cbCampoBusqueda.SelectedItem == cbTipo)
            {
                scriptBusquedaPlatos = "select * from Platos where TipoPlato = ( select idTipoPlato from TipoPlatos where nombreTipo like '" + txtValor.Text +"%'); ";
            }
            else {
                scriptBusquedaPlatos = "SELECT * FROM dbo.Platos where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
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
