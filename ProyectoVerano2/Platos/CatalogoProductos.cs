using Proyecto;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVerano2.Platos
{
     class CatalogoProductos
    {
        public List<Productos> GetProductosActives()
        {
            string script = "SELECT * FROM dbo.Productos;";
            DataTable dt = new DataTable();
            dt = BD.RellenarDataTable(dt, ListaProductos.scriptBusquedaProductos);

           

            List<Productos> lstProductos= new List<Productos>();
            for(int i = 0; i < dt.Rows.Count; i++) {
                
                string script2 = "SELECT nombreCategoria from Categorias where idCategoria = " + dt.Rows[i]["categoriaProducto"].ToString();
                DataTable dt2 = new DataTable();
                dt2 = BD.RellenarDataTable(dt2, script2);

                lstProductos.Add(new Productos { ID = (int)dt.Rows[i]["idProducto"], NombreProducto = (string)dt.Rows[i]["nombreProducto"], CantidadStock = (int)dt.Rows[i]["CantidadStock"], CategoriaProducto = (string)dt2.Rows[0][0]});

            }

            return lstProductos;
        }



    }
}
