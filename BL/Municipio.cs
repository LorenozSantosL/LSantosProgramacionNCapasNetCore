using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class Municipio
    {
        public static ML.Result GetByIdEstado(int IdEstado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new  DL.LsantosProgramacionNcapasContext())
                {
                    var municipios = context.Municipios.FromSqlRaw($"MunicipioGetByIdEstado {IdEstado}").ToList();
                    result.Objects = new List<object>();

                    if(municipios != null)
                    {
                        foreach(var objMunicipio in municipios)
                        {
                            ML.Municipio municipio = new ML.Municipio();

                            municipio.IdMunicipio = objMunicipio.IdMunicipio;
                            municipio.Nombre = objMunicipio.Nombre;

                            municipio.Estado = new ML.Estado();
                            municipio.Estado.IdEstado = objMunicipio.IdEstado.Value;

                            result.Objects.Add(municipio);
                        }
                    }
                
                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta" + result.EX;
                throw;
            }

            return result;
        }
    }
}
