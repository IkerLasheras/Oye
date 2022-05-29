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

        /// <summary>
        /// Realiza las consultas para obtener la informacion de todas las bebidas y recorre una por una añadiendo su informacion a una lista de bebebidas
        /// </summary>
        /// <returns>lista de bebidas</returns>
        public List<Bebida> GetBebidasActives()
        {
            string script = "SELECT * FROM dbo.Bebidas;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaBebidas.scriptBusquedaBebidas);
            List<Bebida> lstBebidas = new List<Bebida>();

            for(int i = 0; i < dt.Rows.Count; i++) {
                string script2 = "SELECT nombre from Tipobebida where idTipoBebida = " + dt.Rows[i]["tipoBebida"].ToString();
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstBebidas.Add(new Bebida { ID = (int)dt.Rows[i]["idBebida"], NombreBebida = (string)dt.Rows[i]["nombreBebida"], Precio = float.Parse(dt.Rows[i]["precio"].ToString()) , TipoBebida = (string)dt2.Rows[0][0]});
            }
            return lstBebidas;
        }
    }
}
