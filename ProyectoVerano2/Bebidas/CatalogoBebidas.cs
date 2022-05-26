using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Bebidas
{
     class CatalogoBebidas
    {
        public List<Mesas> GetBebidasActives()
        {
            string script = "SELECT * FROM dbo.Bebidas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaBebidas.scriptBusquedaBebidas);

           

            List<Mesas> lstBebidas = new List<Mesas>();
            for(int i = 0; i < dt.Rows.Count; i++) {
                
                string script2 = "SELECT nombre from Tipobebida where idTipoBebida = " + dt.Rows[i]["tipoBebida"].ToString();
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstBebidas.Add(new Mesas { ID = (int)dt.Rows[i]["idBebida"], NombreBebida = (string)dt.Rows[i]["nombreBebida"], Precio = float.Parse(dt.Rows[i]["precio"].ToString()) , TipoBebida = (string)dt2.Rows[0][0]});

            }


            return lstBebidas;
        }



    }
}
