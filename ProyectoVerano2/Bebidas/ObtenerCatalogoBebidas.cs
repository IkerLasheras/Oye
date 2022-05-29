using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Bebidas
{
     class ObtenerCatalogoMesas
    {
        /// <summary>
        /// Devuelbe el la lista que Devuelve el metodo GetBebidasActives() de la clase catalogoBebidas.
        /// </summary>
        /// <returns>GetBebidasActives() de la clase catalogoBebidas.</returns>
        public List<Bebida> GetBebidasActives()
        {
            CatalogoBebidas catalogoBebidas = new CatalogoBebidas();

            return catalogoBebidas.GetBebidasActives();
        }
    }
}
