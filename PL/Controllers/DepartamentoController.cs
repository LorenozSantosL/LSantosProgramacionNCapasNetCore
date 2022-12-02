using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DepartamentoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Departamento.GetAll();
            ML.Departamento departamento = new ML.Departamento();

            if (result.Correct)
            {
                departamento.Departamentos = result.Objects;   
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta ";
            }


            return View(departamento);
        }

        [HttpGet]
        public ActionResult Form(int? IdDepartamento)
        {
            ML.Departamento departamento = new ML.Departamento();

            departamento.Area = new ML.Area();

            ML.Result resultArea = BL.Area.GetAll();

            if(IdDepartamento ==null)
            {
                departamento.Area.Areas = resultArea.Objects;
                return View(departamento);
            }
            else
            {
                ML.Result result = BL.Departamento.GetById(IdDepartamento.Value);

                if(result.Correct)
                {
                    departamento = (ML.Departamento)result.Object;
                    departamento.Area.Areas = resultArea.Objects;
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al buscar al departamento";
                }
                return View(departamento);

            }

            
        }

        [HttpPost]
        public ActionResult Form(ML.Departamento departamento)
        {
            if(departamento.IdDepartamento == 0)
            {
                ML.Result result = BL.Departamento.Add(departamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error: " + result.Message;
                }
                    
            }
            else
            {
                ML.Result result = BL.Departamento.Update(departamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error: " + result.Message;
                }

            }
            return PartialView("Modal");
        }

        public ActionResult Delete(int IdDepartamento)
        {
            if (IdDepartamento > 0)
            {
                ML.Result result = BL.Departamento.Delete(IdDepartamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error: " + result.Message;
                }
            }
                return PartialView("Modal");
        }
    }
}
