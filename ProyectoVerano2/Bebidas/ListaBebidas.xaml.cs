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
        public static int id = 0;
        public static string scriptBusquedaBebidas = "SELECT * FROM dbo.Bebidas;";
        public ListaBebidas()
        {
            InitializeComponent();
            EnseniarBebidas();
        }

        /// <summary>
        /// Rellena el DataGrid con la informcacion obtenida en la clase obtenerCatalogoBebidas
        /// </summary>
        private void EnseniarBebidas()
        {
            ObtenerCatalogoMesas obtenerCatalogoBebidas = new ObtenerCatalogoMesas();
            List<Bebida> lista = obtenerCatalogoBebidas.GetBebidasActives();

            this.dtgBebidas.ItemsSource = lista;
        }

        /// <summary>
        /// Evento que se activa al cambiar el elemento seleccionado del DataGrid, Guarda el valor de Del ID de la bebida seleccionada y cierra la Ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgBebidas.SelectedIndex != -1)
            {
                Bebida? bebida = this.dtgBebidas.SelectedItem as Bebida;

                id = bebida.ID;
                Close();  
            }
        }

        /// <summary>
        /// Realiza busqueda dependiendo de lo elegido en el ComboBox y enseña las bebidas que coinciden con la consulta
        /// </summary>
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
