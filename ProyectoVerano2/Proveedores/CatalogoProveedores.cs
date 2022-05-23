using Proyecto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Proveedores
{
     class CatalogoProveedores
    {
        public List<Proveedores> GetProveedoresActives()
        {
            string script = "SELECT * FROM dbo.Proveedores;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaProveedores.scriptBusquedaProveedor);

           

            List<Proveedores> lstProveedores= new List<Proveedores>();
            for(int i = 0; i < dt.Rows.Count; i++) {
                
                string script2 = "SELECT nombreCategoria from Categorias where idCategoria = " + dt.Rows[i]["categoriaProveedor"].ToString();
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstProveedores.Add(new Proveedores { ID = (int)dt.Rows[i]["idProveedor"], NombreProveedor = (string)dt.Rows[i]["nombreProveedor"], Telefono = (string)dt.Rows[i]["telefono"], CategoriaProveedor = (string)dt2.Rows[0][0]});

            }

            return lstProveedores;
        }



    }
}
