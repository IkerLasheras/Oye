using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Mesas
{
     class ObtenerCatalogoMesas
    {
        public List<Mesas> GetMesasActives()
        {
            CatalogoMesas catalogoMesas = new CatalogoMesas();

            return catalogoMesas.GetMesasActives();
        }
    }
}
