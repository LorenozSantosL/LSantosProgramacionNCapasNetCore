using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Area
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var areas = context.Areas.FromSqlRaw("AreaGetAll").ToList();

                    result.Objects = new List<object>();

                    if (areas != null)
                    {
                        foreach (var objArea in areas)
                        {
                            ML.Area area = new ML.Area();

                            area.IdArea = objArea.IdArea;
                            area.Nombre = objArea.Nombre;

                            result.Objects.Add(area);
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
