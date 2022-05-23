using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Platos
{
     class ObtenerCatalogoProductos
    {
        public List<Productos> GetProductosActives()
        {
            CatalogoProductos catalogoProductos = new CatalogoProductos();

            return catalogoProductos.GetProductosActives();
        }
    }
}
