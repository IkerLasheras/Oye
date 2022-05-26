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

namespace ProyectoVerano2.Mesas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ListaMesas : Window
    {


        // public static string campoBusqueda = cbCampoBusqueda.SelectedItem.ToString();


        public static int id = 0;
        public static string scriptBusquedaMesas = "SELECT * FROM dbo.Mesas;";
        public ListaMesas()
        {
            InitializeComponent();
            EnseniarBebidas();
        }

        private void EnseniarBebidas()
        {
            ObtenerCatalogoMesas obtenerCatalogoMesas = new ObtenerCatalogoMesas();
            List<Mesas> lista = obtenerCatalogoMesas.GetMesasActives();

            this.dtgMesas.ItemsSource = lista;
        }

        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgMesas.SelectedIndex != -1)
            {
                Mesas? bebida = this.dtgMesas.SelectedItem as Mesas;

                id = bebida.ID;
                Close();
                
            }
        }



        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbUbicacion)
            {
                scriptBusquedaMesas = "SELECT * FROM dbo.Mesas where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else 
            {
                scriptBusquedaMesas = "SELECT * FROM dbo.Mesas where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
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
