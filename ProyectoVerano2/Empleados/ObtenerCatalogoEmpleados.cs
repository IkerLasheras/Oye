using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Empleados
{
    class ObtenerCatalogoEmpleados
    {

        /// <summary>
        /// Devuelbe el la lista que devuelve el metodo .
        /// </summary>
        /// <returns>GetBebidasActives() de la clase catalogoBebidas</return
        public List<Empleados> GetEmpleadosActives()
        {
            CatalogoEmpleados catalogoEmpleados = new CatalogoEmpleados();

            return catalogoEmpleados.GetEmpleadosActives();
        }
    }
}
