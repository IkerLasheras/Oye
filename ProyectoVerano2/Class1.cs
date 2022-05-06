using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2
{
    public class ImageClass
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        byte[] gimagen;

        public byte[] imagen
        {
            get { return gimagen; }
            set { gimagen = value; }
        }
    }
}
