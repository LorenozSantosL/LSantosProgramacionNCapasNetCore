using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
           
            ML.Result resultProveedores = BL.Proveedor.GetAll();
            ML.Producto producto = new ML.Producto();
            producto.Proveedor = new ML.Proveedor();
            ML.Result result = BL.Producto.GetAll(producto);
          

            if (result.Correct)
            {
                producto.Proveedor.Proveedores = resultProveedores.Objects;
                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta";
            }

            return View(producto);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Producto producto)
        {
            
            ML.Result result = BL.Producto.GetAll(producto);

            producto.Proveedor = new ML.Proveedor();
            ML.Result resultProveedores = BL.Proveedor.GetAll();


            if (result.Correct)
            {
                producto.Proveedor.Proveedores = resultProveedores.Objects;

                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta";
            }

            return View(producto);
        }

        [HttpGet]
        public ActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();
            producto.Proveedor = new ML.Proveedor();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            ML.Result resultProveedores = BL.Proveedor.GetAll();
            ML.Result resultArea = BL.Area.GetAll();

            if(IdProducto == null)
            {
                producto.Proveedor.Proveedores = resultProveedores.Objects;
                producto.Departamento.Area.Areas = resultArea.Objects;

                return View(producto);
            }
            else
            {
                ML.Result result = BL.Producto.GetById(IdProducto.Value);

                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;
                    producto.Proveedor.Proveedores = resultProveedores.Objects;
                    producto.Departamento.Area.Areas= resultArea.Objects;
                    ML.Result resultDepartaamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
                    producto.Departamento.Departamentos = resultDepartaamento.Objects;

                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar el producto seleccionado";
                }


                return View(producto);
            }

            
        }
        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            IFormFile image = Request.Form.Files["inputImagen"];

            if (image != null)
            {
                byte[] ImagenBytes = ConvertToBytes(image);

                producto.Imagen = Convert.ToBase64String(ImagenBytes);
            }

            if(producto.IdProducto == 0)
            {
                ML.Result result = BL.Producto.Add(producto);

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
                ML.Result result = BL.Producto.Update(producto);

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

        public ActionResult Delete(int IdProducto)
        {
            if(IdProducto > 0)
            {
                ML.Result result = BL.Producto.Delete(IdProducto);

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


        public JsonResult GetDepartamento(int IdArea)
        {
            var result = BL.Departamento.GetByIdArea(IdArea);

            return Json(result.Objects);
        }


        public static Byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }
    }


}
