using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int IdMunicipio)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var colonias = context.Colonia.FromSqlRaw($"ColoniaGetByIdMunicipio {IdMunicipio}").ToList();

                    result.Objects = new List<object>();

                    if(colonias != null)
                    {
                        foreach(var objColonia in colonias)
                        {
                            ML.Colonia colonia = new ML.Colonia();

                            colonia.IdColonia = objColonia.IdColonia;
                            colonia.Nombre = objColonia.Nombre;
                            colonia.CodigoPostal = objColonia.CodigoPostal;

                            colonia.Municipio = new ML.Municipio();
                            colonia.Municipio.IdMunicipio = objColonia.IdMunicipio.Value;

                            result.Objects.Add(colonia);
                            
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
