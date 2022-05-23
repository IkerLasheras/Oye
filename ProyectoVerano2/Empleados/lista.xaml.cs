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
        public Lista()
        {
            InitializeComponent();
        }

        private void EnseniarEmpleados()
        {
            ObtenerCatalogoEmpleados obtenerCatalogoEmpleados = new ObtenerCatalogoEmpleados();
            List<Empleados> lista = obtenerCatalogoEmpleados.GetEmpleadosActives();

            this.dtgEmpleadpos.ItemsSource = lista;
        }

        private void dtgEmpleadpos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgEmpleadpos.SelectedIndex != -1)
            {
                Empleados empleado = this.dtgEmpleadpos.SelectedItem as Empleados;

                WinInsertarEmpleados winInsertarEmpleados = new WinInsertarEmpleados();
                winInsertarEmpleados.EnseniarEmpleadoEnPantalla(empleado.ID);
                Close();
                
            }
        }
    }
}
