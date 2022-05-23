using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Bebidas
{
     class ObtenerCatalogoBebidas
    {
        public List<Bebidas> GetBebidasActives()
        {
            CatalogoBebidas catalogoBebidas = new CatalogoBebidas();

            return catalogoBebidas.GetBebidasActives();
        }
    }
}
