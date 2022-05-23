using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Platos
{
     class CatalogoPlatos
    {
        public List<Platos> GetPlatosActives()
        {
            string script = "SELECT * FROM dbo.Platos;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaPlatos.scriptBusquedaPlatos);

           

            List<Platos> lstPlatos= new List<Platos>();
            for(int i = 0; i < dt.Rows.Count; i++) {
                
                string script2 = "SELECT nombretipo from TipoPlatos where idTipoPlato = " + dt.Rows[i]["tipoPlato"].ToString();
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstPlatos.Add(new Platos { ID = (int)dt.Rows[i]["idPlato"], NombrePlato = (string)dt.Rows[i]["nombrePlato"], Precio = float.Parse(dt.Rows[i]["precio"].ToString()) , TipoPlato = (string)dt2.Rows[0][0]});

            }

            return lstPlatos;
        }



    }
}
