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

namespace ProyectoVerano2.TPV
{
    /// <summary>
    /// Lógica de interacción para TPV.xaml
    /// </summary>
    public partial class TPV : Window
    {
        public IList<Button> BotonesTipos{ get; set; }
        public IList<Button> BotonesCategorias { get; set; }
        public IList<Button> BotonesPlatos { get; set; }
        public TPV()
        {
            crearBotonesPlatos();
            //crearBotonesCategoria();
            InitializeComponent();
            

        }
        public void crearBotonesPlatos()
        {
            BotonesPlatos = new List<Button>();

            for (int i = 0; i < 10; i++)
            {


                BotonesPlatos.Add(new Button
                {

                    Name = "btn" + i.ToString(),
                    Width = 200,
                    Height = 100,
                    Margin = new Thickness(10),

                });

               DataContext = this;
            }
        }

        //public void crearBotonesCategoria()
        //{
        //    BotonesCategorias = new List<Button>();

        //    for (int i = 0; i < 5; i++)
        //    {


        //        BotonesCategorias.Add(new Button
        //        {

        //            Name = "btn" + i.ToString(),
        //            Width = 200,
        //            Height = 100,
        //            Margin = new Thickness(10),

        //        });

         
        //    }
        //}
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
