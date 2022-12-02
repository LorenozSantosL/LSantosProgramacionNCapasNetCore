using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {

        public static ML.Result Add(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var addDepartamento = context.Database.ExecuteSqlRaw($"DepartamentoAdd {departamento.Nombre}, {departamento.Area.IdArea} ");

                    if(addDepartamento > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha agregado el departamento";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al agregar el departamento";
                throw;
            }

            return result;
        }
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var departamentos = context.Departamentos.FromSqlRaw("DepartamentoGetAll").ToList();

                    result.Objects = new List<object>();

                    if(departamentos != null)
                    {
                        foreach(var objDepartamento in departamentos)
                        {
                            ML.Departamento departamento = new ML.Departamento();

                            departamento.IdDepartamento = objDepartamento.IdDepartamento;
                            departamento.Nombre = objDepartamento.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = objDepartamento.IdArea;
                            departamento.Area.Nombre = objDepartamento.AreaNombre;

                            result.Objects.Add(departamento);
                        }
                    }
                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrio un erro al hacer la consulta";
                result.EX = ex;
                throw; 
            }

            return result; 
        }

        public static ML.Result GetById(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var objDepartamento = context.Departamentos.FromSqlRaw($"DepartamentoGetById { IdDepartamento}").AsEnumerable().FirstOrDefault();

                    result.Object = new List<object>();

                    if(objDepartamento != null)
                    {
                        ML.Departamento departamento = new ML.Departamento();

                        departamento.IdDepartamento = objDepartamento.IdDepartamento;
                        departamento.Nombre = objDepartamento.Nombre;
                        departamento.Area = new ML.Area();

                        departamento.Area.IdArea = objDepartamento.IdArea;
                        departamento.Area.Nombre = objDepartamento.AreaNombre;

                        result.Object = departamento;
                    }

                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta";
                throw;
            }

            return result;
        }

        public static ML.Result Update(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var updateDepartamento = context.Database.ExecuteSqlRaw($"DepartamentoUpdate {departamento.IdDepartamento}, '{departamento.Nombre}', {departamento.Area.IdArea} ");

                    if (updateDepartamento > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha actualizado el departamento";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al actualizar el departamento";
                throw;
            }

            return result;
        }

        public static ML.Result Delete(int IdDepartamento)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var deletDepartamento = context.Database.ExecuteSqlRaw($"DepartamentoDelete {IdDepartamento}");

                    if(deletDepartamento > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha elimnado el departamento";
                    }

                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;

                result.Message = "No se pudo eliminar el departamento";
                throw;
            }

            return result;
        }

        public static ML.Result GetByIdArea(int IdArea)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var departamentos = context.Departamentos.FromSqlRaw($"DepartamentoGetByIdArea {IdArea}").AsEnumerable().ToList();

                    result.Objects = new List<object>();

                    if(departamentos != null)
                    {
                        foreach(var objDepartamento in departamentos)
                        {
                            ML.Departamento departamento = new ML.Departamento();

                            departamento.IdDepartamento = objDepartamento.IdDepartamento;
                            departamento.Nombre = objDepartamento.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = objDepartamento.IdArea;
                            departamento.Area.Nombre = objDepartamento.AreaNombre;

                            result.Objects.Add(departamento);
                        }
                    }
                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;

                result.Message = "No se pudo hacer la consulta del departamento";
                throw;
            }

            return result;
        }
    }
}
