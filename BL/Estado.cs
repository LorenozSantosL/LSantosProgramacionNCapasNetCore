using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result GetByIdPais(int IdPais)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var estados = context.Estados.FromSqlRaw($"EstadoGetByIdPais {IdPais}").ToList();

                    result.Objects = new List<object>();

                    if(estados != null)
                    {
                        foreach( var objEstados in estados )
                        {
                            ML.Estado estado = new ML.Estado();

                            estado.IdEstado = objEstados.IdEstado;
                            estado.Nombre = objEstados.Nombre;

                            estado.Pais = new ML.Pais();

                            estado.Pais.IdPais = objEstados.IdPais.Value;

                            result.Objects.Add(estado);
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
