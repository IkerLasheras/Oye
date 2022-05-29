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
        public static int id = 0;
        public static string scriptBusquedaMesas = "SELECT * FROM dbo.Mesas;";
        public ListaMesas()
        {
            InitializeComponent();
            EnseniarMesas();
        }

        /// <summary>
        /// Rellena el DataGrid con la informcacion obtenida en la clase obtenerCatalogoMesas
        /// </summary>
        private void EnseniarMesas()
        {
            ObtenerCatalogoMesas obtenerCatalogoMesas = new ObtenerCatalogoMesas();
            List<Mesas> lista = obtenerCatalogoMesas.GetMesasActives();

            this.dtgMesas.ItemsSource = lista;
        }


        /// <summary>
        /// Evento que se activa al cambiar el elemento seleccionado del DataGrid, Guarda el valor de Del ID de la bebida seleccionada y cierra la Ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgMesas.SelectedIndex != -1)
            {
                Mesas? mesas = this.dtgMesas.SelectedItem as Mesas;

                id = mesas.ID;
                Close();
            }
        }

        /// <summary>
        /// Realiza busqueda dependiendo de lo elegido en el ComboBox y enseña las mesas que coinciden con la consulta
        /// </summary>
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
            EnseniarMesas();
        }

        /// <summary>
        /// Evento de click en el boton Salir. Cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Evento que se activa al levantar una tecla tras pulsarla, realiza las busquedas mediante el metdod Buscar().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_KeyUp(object sender, KeyEventArgs e)
        {
            Buscar();
        }
    }
}
