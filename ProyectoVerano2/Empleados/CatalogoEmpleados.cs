using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Empleados
{
     class CatalogoEmpleados
    {
        public List<Empleados> GetEmpleadosActives()
        {
            string script = "SELECT * FROM dbo.Empleados;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, script);

           

            List<Empleados> lstEmpleados = new List<Empleados>();
            for(int i = 0; i < dt.Rows.Count; i++) {

                string script2 = "SELECT nombreTipoEmpleado from tipoEmpleado where idTipoEmpleado = " + (string)dt.Rows[i]["tipoEmpleado"];
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstEmpleados.Add(new Empleados { ID = (int)dt.Rows[i]["idEmpleado"], Nombre = (string)dt.Rows[i]["nombre"], Apellido = (string)dt.Rows[i]["apellido1"], DNI = (string)dt.Rows[i]["dni"], Tipo = (string)dt2.Rows[0][0] });

            }

            return lstEmpleados;
        }



    }
}
