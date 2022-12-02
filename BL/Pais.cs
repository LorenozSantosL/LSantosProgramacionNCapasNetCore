using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Pais
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var paises = context.Pais.FromSqlRaw("PaisGetAll").ToList();

                    result.Objects = new List<object>();

                    if(paises != null)
                    {
                        foreach(var objPais in paises)
                        {
                            ML.Pais pais = new ML.Pais();

                            pais.IdPais = objPais.IdPais;
                            pais.Nombre = objPais.Nombre;


                            result.Objects.Add(pais);
                        }
                    }
                }

                result.Correct = true;
            }
            catch (Exception ex)
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
