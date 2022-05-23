using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Proveedores
{
     class ObtenerCatalogoProveedores
    {
        public List<Proveedores> GetProveedoresActives()
        {
            CatalogoProveedores catalogoProveedores = new CatalogoProveedores();

            return catalogoProveedores.GetProveedoresActives();
        }
    }
}
