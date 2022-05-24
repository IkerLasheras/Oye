using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static string script = "";
        public static bool nose = true;
        public IList<Button> SubTipoBotones { get; set; }
        public IList<Button> TipoBotones { get; set; }



        public TPV()
        {
            if(!nose )
            {
                CrearTipoBotones(script);
            }
           
           
                    InitializeComponent();
        }

        public void CrearSubTipoBotones()
        {
            SubTipoBotones = new List<Button>();

            for (int i = 0; i < 15; i++)
            {


                Button btnSubTipo = new Button();
                btnSubTipo.Content = i;
                btnSubTipo.Width = 150;
                btnSubTipo.Height = 50;
                btnSubTipo.Click += btn_click;

                SubTipoBotones.Add(btnSubTipo);

                DataContext = this;
            }
            this.AddChild(TipoBotones);
        }

        public void CrearTipoBotones(string script)
        {
            TipoBotones = new List<Button>();

            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

            for (int i = 0;i <1; i++)
            {
                Button btnTipo = new Button();
                btnTipo.Content = i;
                btnTipo.Width = 150;
                btnTipo.Height = 50;
                btnTipo.Click += btnTipo_click;

                TipoBotones.Add(btnTipo);

                DataContext = this;
            }
           
        }

        private void btn_click(object sender, RoutedEventArgs e)
        {

            int[] datos = { 1, 5, 7, 8, 10 };
            Button button = (Button)sender;
            int content = Convert.ToInt32(button.Content);
            for (int i = 0; i < datos.Length; i++)
            {
                if (content == datos[i])
                {
                    button.Content = "aaaaaaa";
                }
            }


        }
        private void btnTipo_click(object sender, RoutedEventArgs e)
        {

            int[] datos = { 1, 5, 7, 8, 10 };
            Button button = (Button)sender;
            int content = Convert.ToInt32(button.Content);
            for (int i = 0; i < datos.Length; i++)
            {
                if (content == datos[i])
                {
                    button.Content = "aaaaaaa";
                }
            }


        }

      
        private void btnPlatos_Click(object sender, RoutedEventArgs e)
        {
            script = "select Nombretipo from tipoPlatos";
            nose = false;
            Window tpv = new TPV();
        }

        private void btnBebidas_Click(object sender, RoutedEventArgs e)
        {
            script = "select Nombre from tipoBebida";
            nose = false;
            Window tpv = new TPV();

        }
    }
}
