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

namespace ProyectoVerano2.Bebidas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ListaBebidas : Window
    {


        // public static string campoBusqueda = cbCampoBusqueda.SelectedItem.ToString();


        public static int id = 0;
        public static string scriptBusquedaBebidas = "SELECT * FROM dbo.Bebidas;";
        public ListaBebidas()
        {
            InitializeComponent();
            EnseniarBebidas();
        }

        private void EnseniarBebidas()
        {
            ObtenerCatalogoMesas obtenerCatalogoBebidas = new ObtenerCatalogoMesas();
            List<Mesas> lista = obtenerCatalogoBebidas.GetBebidasActives();

            this.dtgBebidas.ItemsSource = lista;
        }

        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgBebidas.SelectedIndex != -1)
            {
                Mesas? bebida = this.dtgBebidas.SelectedItem as Mesas;

                id = bebida.ID;
                Close();
                
            }
        }



        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbNombre)
            {
                scriptBusquedaBebidas = "SELECT * FROM dbo.Bebidas where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else if(cbCampoBusqueda.SelectedItem == cbTipoBebida)
            {
                scriptBusquedaBebidas = "select * from bebidas where tipobebida = ( select idTipoBebida from TipoBebida where nombre like '" + txtValor.Text +"%'); ";
            }
            else {
                scriptBusquedaBebidas = "SELECT * FROM dbo.bebidas where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
            }
            EnseniarBebidas();
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
