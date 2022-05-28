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

namespace ProyectoVerano2
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnWinAniadirEmpleados_Click(object sender, RoutedEventArgs e)
        {
            Window winInsertarEmpleados = new WinInsertarEmpleados();

            winInsertarEmpleados.ShowDialog();
        }

        private void btnWinAniadirpProductos_Click(object sender, RoutedEventArgs e)
        {
            Window winProductos = new WinProductos();

            winProductos.ShowDialog();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnWinaniadirPlatos_Click(object sender, RoutedEventArgs e)
        {
            Window winPlatos = new WinPlatos();

            winPlatos.ShowDialog();
        }

        private void btnWinaniadirBebidas_Click(object sender, RoutedEventArgs e)
        {
            Window winBebidas = new WinBebidas();

            winBebidas.ShowDialog();
        }

        private void btnWinaniadirProveedor_Click(object sender, RoutedEventArgs e)
        {
            Window winProveedores = new WinProveedores();

            winProveedores.ShowDialog();
        }

        private void btnWinaniadirTPV_Click(object sender, RoutedEventArgs e)
        {
            Window winTPV = new TPV.TPV2();
            winTPV.ShowDialog();

        }

        private void btnWinaniadirMesas_Click(object sender, RoutedEventArgs e)
        {
            Window winMesas = new WinMesas();
            winMesas.ShowDialog();
        }
    }
}
