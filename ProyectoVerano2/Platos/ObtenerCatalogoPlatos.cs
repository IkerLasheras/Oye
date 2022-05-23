using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Platos
{
     class ObtenerCatalogoPlatos
    {
        public List<Platos> GetPlatosActives()
        {
            CatalogoPlatos catalogoPlatos = new CatalogoPlatos();

            return catalogoPlatos.GetPlatosActives();
        }
    }
}
