using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Mesas
{
     class CatalogoMesas
    {
        public List<Mesas> GetMesasActives()
        {
            string script = "SELECT * FROM dbo.Bebidas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaMesas.scriptBusquedaMesas);

           

            List<Mesas> lstMesas = new List<Mesas>();
            for(int i = 0; i < dt.Rows.Count; i++) {

                lstMesas.Add(new Mesas { ID = (int)dt.Rows[i]["idMesa"], Ubicacion = (string)dt.Rows[i]["ubicacion"]});

            }


            return lstMesas;
        }



    }
}
