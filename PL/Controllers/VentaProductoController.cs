using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace PL.Controllers
{
    public class VentaProductoController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public VentaProductoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }



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


        public ActionResult AgregarACarrito(int? IdProducto)
        {
            ML.VentaProducto ventaproducto = new ML.VentaProducto();
            ventaproducto.VentaProductos = new List<object>();
            bool productoAgregado = false;

            if(HttpContext.Session.GetString("VentaProducto") == null)
            {
                ventaproducto.Producto = new ML.Producto();
                ventaproducto.Producto.IdProducto = IdProducto.Value;
                ventaproducto.Cantidad = 1;

                ML.Result resultProducto = BL.Producto.GetById(IdProducto.Value);
                ventaproducto.Producto = (ML.Producto)resultProducto.Object;

                ventaproducto.VentaProductos.Add(ventaproducto);
                HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaproducto.VentaProductos)); //serealizacion  //te permite comvertir tu modelo a tipo json
                ViewBag.Message = "Se ha Agregado al producto";
                return PartialView("Modal");
               
            }
            else
            {

                var carritoProdutos = JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("VentaProducto"));

                if(carritoProdutos != null)
                {
                    bool existe = false;

                    foreach(var producto in carritoProdutos)
                    {
                        ML.VentaProducto ventaProductoObj = JsonConvert.DeserializeObject<ML.VentaProducto>(producto.ToString());
                        ventaproducto.VentaProductos.Add(ventaProductoObj);
                    }

                    foreach(ML.VentaProducto verificaProducto in ventaproducto.VentaProductos)
                    {
                        if(verificaProducto.Producto.IdProducto == IdProducto.Value)
                        {
                            existe = true;
                            verificaProducto.Cantidad++;
                        }

                    }

                    if (!existe)
                    {

                        ventaproducto.Producto = new ML.Producto();
                        ventaproducto.Producto.IdProducto = IdProducto.Value;
                        ventaproducto.Cantidad = 1;

                        ML.Result resultProducto = BL.Producto.GetById(IdProducto.Value);
                        ventaproducto.Producto = (ML.Producto)resultProducto.Object;
                        ventaproducto.VentaProductos.Add(ventaproducto);
                        HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaproducto.VentaProductos));
                        productoAgregado = true;
                        ViewBag.Message = "Se ha Agregado una cantidad del producto";
                    }
                    else
                    {
                        HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaproducto.VentaProductos));
                        productoAgregado=false;
                        ViewBag.Message = "Se ha Agregado una cantidad del producto";
                    }


                }

                

                return PartialView("Modal");
            }


            

           


        }

        public IActionResult Carrito(ML.VentaProducto ventaProducto)
        {
            if(HttpContext.Session.GetString("VentaProducto") == null)
            {
                ViewBag.Message = "No tiene productos en el carrito";

                return View("Modal");
            }
            else
            {
                var ventaSession = JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("VentaProducto"));
                ventaProducto.VentaProductos = new List<object>();

                foreach(var  obj in ventaSession)
                {
                    ML.VentaProducto ventaProductoObj = JsonConvert.DeserializeObject<ML.VentaProducto>(obj.ToString());
                    ventaProducto.VentaProductos.Add(ventaProductoObj);
                }
            }

            return View(ventaProducto);

            
        }

        public IActionResult Agregar(int IdProducto)
        {
            ML.VentaProducto ventaproducto = new ML.VentaProducto();
            ventaproducto.VentaProductos = new List<object>();
            int cantidad =0;
            var carritoProdutos = JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("VentaProducto"));

            if (carritoProdutos != null)
            {
                bool esMayor = false;
                bool existe = false;

                foreach (var producto in carritoProdutos)
                {
                    ML.VentaProducto ventaProductoObj = JsonConvert.DeserializeObject<ML.VentaProducto>(producto.ToString());
                    ventaproducto.VentaProductos.Add(ventaProductoObj);
                }

                foreach (ML.VentaProducto verificaProducto in ventaproducto.VentaProductos)
                {
                    if (verificaProducto.Producto.IdProducto == IdProducto)
                    {
                        existe = true;
                        verificaProducto.Cantidad++;


                        if(verificaProducto.Cantidad > verificaProducto.Producto.Stock)
                        {
                            verificaProducto.Cantidad = verificaProducto.Cantidad - 1;
                            HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaproducto.VentaProductos));
                            ViewBag.Message = "La cantidad es mayor a la del Stock";
                        }
                        else
                        {
                            HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaproducto.VentaProductos));
                            ViewBag.Message = "Se aumento la cantidad del producto";
                        }
                        
                        
                    }


                }

            } 

            return PartialView("Modal");
        }

        public IActionResult Desagregar (int IdProducto)
        {
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentaProductos = new List<object>();

            var carritoProdutos = JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("VentaProducto"));
            var indice = 0;
           
            if (carritoProdutos != null)
            {
                bool esCero = false;

                foreach (var producto in carritoProdutos)
                {
                    ML.VentaProducto ventaProductoObj = JsonConvert.DeserializeObject<ML.VentaProducto>(producto.ToString());
                    ventaProducto.VentaProductos.Add(ventaProductoObj);
                }

                foreach (ML.VentaProducto verificaProducto in ventaProducto.VentaProductos)
                {
                    if (verificaProducto.Producto.IdProducto == IdProducto)
                    {

                        
                        verificaProducto.Cantidad = verificaProducto.Cantidad - 1;
                      

                        if(verificaProducto.Cantidad == 0)
                        {
                            indice = ventaProducto.VentaProductos.IndexOf(verificaProducto);
                            esCero = true;
                            
                        }
                        else
                        {
                            HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                            ViewBag.Message = "Se ha eliminado una cantidad del producto";
                            esCero = false;
                        }
                        
                        
                    }

                    


                }
                if (esCero)
                {
                    ventaProducto.VentaProductos.RemoveAt(indice);
                    HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                    ViewBag.Message = "Se ha eliminado el producto";
                }

            }

            return PartialView("Modal");
        }

        public IActionResult Eliminar( int IdProducto)
        {
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentaProductos = new List<object>();
            var carritoProdutos = JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("VentaProducto"));
            var indice = 0;
            if (carritoProdutos != null)
            {
                bool existe = false;

                foreach (var producto in carritoProdutos)
                {
                    ML.VentaProducto ventaProductoObj = JsonConvert.DeserializeObject<ML.VentaProducto>(producto.ToString());
                    ventaProducto.VentaProductos.Add(ventaProductoObj);
                }

                foreach (ML.VentaProducto verificaProducto in ventaProducto.VentaProductos)
                {
                    if (verificaProducto.Producto.IdProducto == IdProducto)
                    {
                       // indice = ventaMateria.VentaMaterias.IndexOf(materia);
                        indice = ventaProducto.VentaProductos.IndexOf(verificaProducto);
                        existe = true;                    
                        //
                    }


                }

                if (existe)
                {
                    ventaProducto.VentaProductos.RemoveAt(indice);
                    HttpContext.Session.SetString("VentaProducto", JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                    ViewBag.Message = "Se ha eliminado una cantidad del producto";
                }



            }

            return PartialView("Modal");
        }
    }
}
