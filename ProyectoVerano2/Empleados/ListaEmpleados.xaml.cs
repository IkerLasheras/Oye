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

namespace ProyectoVerano2.Empleados
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Lista : Window
    {
        public static int id = 0;
        public static string scriptBusquedaEmpleados = "SELECT * FROM dbo.Empleados;";
        public Lista()
        {
                
            InitializeComponent();
            EnseniarEmpleados();
        }

        /// <summary>
        /// Rellena el DataGrid con la informcacion obtenida en la clase obtenerCatalogoEmpleados
        /// </summary>
        private void EnseniarEmpleados()
        {
            ObtenerCatalogoEmpleados obtenerCatalogoEmpleados = new ObtenerCatalogoEmpleados();
            List<Empleados> lista = obtenerCatalogoEmpleados.GetEmpleadosActives();

            this.dtgEmpleados.ItemsSource = lista;
        }

        /// <summary>
        /// Evento que se activa al cambiar el elemento seleccionado del DataGrid, Guarda el valor de Del ID de la bebida seleccionada y cierra la Ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgEmpleados.SelectedIndex != -1)
            {
                Empleados empleado = this.dtgEmpleados.SelectedItem as Empleados;
                WinInsertarEmpleados winInsertarEmpleados = new WinInsertarEmpleados();
                id = dtgEmpleados.SelectedIndex;
                winInsertarEmpleados.EnseniarEmpleadoEnPantalla(empleado.ID);
                Close();
            }
        }

        /// <summary>
        /// Realiza busqueda dependiendo de lo elegido en el ComboBox y enseña las bebidas que coinciden con la consulta
        /// </summary>
        private void Buscar()
        {
            if (cbCampoBusqueda.SelectedItem == cbNombre || cbCampoBusqueda.SelectedItem == cbApellido || cbCampoBusqueda.SelectedItem == cbDni)
            {
                scriptBusquedaEmpleados = "SELECT * FROM dbo.Empleados where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " like '" + txtValor.Text + "%';";
            }
            else if (cbCampoBusqueda.SelectedItem == cbTipo)
            {
                scriptBusquedaEmpleados = "select * from Empleados where TipoEmpleado = ( select idTipoEmpleado from TipoEmpleado where nombreTipoEmpleado like '" + txtValor.Text + "%'); ";
            }
            else
            {
                scriptBusquedaEmpleados = "SELECT * FROM dbo.Empleados where " + cbCampoBusqueda.SelectedValue.ToString().Split(" ")[1] + " = " + txtValor.Text + ";";
            }
            EnseniarEmpleados();
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
