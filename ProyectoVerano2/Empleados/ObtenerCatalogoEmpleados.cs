using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Empleados
{
     class ObtenerCatalogoEmpleados
    {
        public List<Empleados> GetEmpleadosActives()
        {
            CatalogoEmpleados catalogoEmpleados = new CatalogoEmpleados();

            return catalogoEmpleados.GetEmpleadosActives();
        }
    }
}
