using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var proveedores = context.Proveedors.FromSqlRaw("ProveedorGetAll").ToList();

                    result.Objects = new List<object>();

                    if(proveedores != null)
                    {
                        foreach(var objProveedor in proveedores)
                        {
                            ML.Proveedor proveedor = new ML.Proveedor();

                            proveedor.IdProveedor = objProveedor.IdProveedor;
                            proveedor.Telefono = objProveedor.Telefono;

                            result.Objects.Add(proveedor);
                        }
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta";
                throw;
            }
            return result;
        }
    }
}
