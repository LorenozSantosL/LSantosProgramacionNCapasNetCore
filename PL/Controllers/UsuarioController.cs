using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            //ML.Result result = BL.Usuario.GetAll(usuario);

            ML.Result result = new ML.Result();

            result.Objects = new List<object>();



            try
            {
                string urlAPI = _configuration["UrlAPI"];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Usuario/GetAll");  //equivale al result = Bl.Usuario.GetAll();

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if(resultServicio.IsSuccessStatusCode) //validamos el estado de la petición
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());

                            result.Objects.Add(resultItemList);
                        }
                        result.Correct = true;
                        
                    }
                    

                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
                
           
            if (result.Correct)
            {
              //guardamos la lista de objetos en una lista de usuarios 
               
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Usuarios = result.Objects;

                return View(usuario);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta ";
                return PartialView("Modal");
            }

            

        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();

           

            result.Objects = new List<object>();


            usuario.Rol = new ML.Rol();
            // ML.Result result = BL.Usuario.GetAll(usuario);try{
            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress =new  Uri(urlAPI);

                    var responseTask = client.GetAsync("Usuario/GetAll/" + usuario.Nombre + usuario.ApellidoPaterno + usuario.Rol.IdRol);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if(resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());

                            result.Objects.Add(resultItemList);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            if (result.Correct)
            {
                usuario.Rol = new ML.Rol();
                ML.Result resultRol = BL.Rol.GetAll();
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Usuarios = result.Objects;   //guardamos la lista de objetos en una lista de usuarios 
                return View(usuario);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta ";
                return View();
            }



        }

        [HttpGet]
        public ActionResult Form(int? IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            ML.Result resultPais = BL.Pais.GetAll();
            ML.Result resultRol = BL.Rol.GetAll();


            if (IdUsuario == null)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                return View(usuario);
            }
            else
            {
                //ML.Result result = BL.Usuario.GetById(IdUsuario.Value);

                ML.Result result = new ML.Result();
                try
                {
                    string urlAPI = _configuration["UrlAPI"];

                    using(var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);

                        var responseTask = client.GetAsync("Usuario/GetById/" + IdUsuario);

                        responseTask.Wait();

                        var resultServicio = responseTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            ML.Usuario usuarioItem = new ML.Usuario();
                            usuarioItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                            result.Object = usuarioItem;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                        }
                    }

                }
                catch(Exception ex)
                {
                    result.Correct = false;
                    result.Message = ex.Message;
                }

                if (result.Correct)
                {

                    usuario = (ML.Usuario)result.Object;
                    usuario.Rol.Roles = resultRol.Objects;

                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

                    ML.Result resulEstado = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = resulEstado.Objects;

                    ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                    ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;

                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar al usuario seleccionado";
                }
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            IFormFile image = Request.Form.Files["inputImagen"];


            if(image != null)
            {
                byte[] ImagenBytes = ConvertToBytes(image);

                usuario.Imagen = Convert.ToBase64String(ImagenBytes);
            }
               
            


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors);
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

                ML.Result resultPais = BL.Pais.GetAll();
                ML.Result resultRol = BL.Rol.GetAll();

                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                return View(usuario);

            }

            else
            {
                if (usuario.IdUsuario == 0)
                {
                    //HttpPostedFileBase FileBase = Request.Files["inputImagen"];  obtener un archivo en este caso de imagen


                    //if (FileBase.ContentLength > 0)
                    //{
                    //    usuario.Imagen = ConvertToBytes(FileBase);
                    //}

                    ML.Result result = new ML.Result();

                    try
                    {
                        string urlAPI = _configuration["UrlAPI"];
                        using(var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(urlAPI);

                            var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add", usuario);

                            postTask.Wait();

                            var resultServicio = postTask.Result;

                            if (resultServicio.IsSuccessStatusCode)
                            {
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.Message = "Ocurrio un error al agregar el usuairo";
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        result.Correct = false;
                        result.Message = ex.Message;
                    }

                    //ML.Result result = BL.Usuario.Add(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = "Se ha agregao el usuario";
                    }
                    else
                    {
                        ViewBag.Messsage = "Error:  " + result.Message;
                    }
                }

                else
                {
                    //HttpPostedFileBase FileBase = Request.Files["inputImagen"];


                    //if (FileBase.ContentLength > 0)
                    //{
                    //    usuario.Imagen = ConvertToBytes(FileBase);
                    //}

                    ML.Result result = new ML.Result();


                    //ML.Result result = BL.Usuario.Update(usuario);

                    try
                    {
                        string urlAPI = _configuration["UrlAPI"];
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(urlAPI);

                            var postTask = client.PutAsJsonAsync<ML.Usuario>("Usuario/Update/"+ usuario.IdUsuario, usuario);

                            postTask.Wait();

                            var resultServicio = postTask.Result;

                            if (resultServicio.IsSuccessStatusCode)
                            {
                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.Message = "Ocurrio un error al agregar el usuairo";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        result.Correct = false;
                        result.Message = ex.Message;
                    }
                    if (result.Correct)
                    {
                        ViewBag.Message = "Se ha actualiado el usuario";
                    }
                    else
                    {
                        ViewBag.Message = "Error: " + result.Message;
                    }
                }
                return PartialView("Modal");
            }

            
        }


        public ActionResult Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            if (IdUsuario > 0)
            {

                try
                {
                    string urlAPI = _configuration["UrlAPI"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);
                        var responseTask = client.DeleteAsync("Usuario/Delete/" + IdUsuario);

                        responseTask.Wait();

                        var resultServicio = responseTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            result.Correct= true;
                        }
                    }

                }
                catch(Exception ex)
                {
                    result.Correct = false;
                    result.Message = ex.Message;
                }
                //ML.Result result = BL.Usuario.Delete(IdUsuario);

                if (result.Correct)
                {
                    ViewBag.Message = "Se ha elimnado el Usuario";
                    

                }
                
            }
            return PartialView("Modal");


        }


        public JsonResult GetEstado(int IdPais)
        {
            var result = BL.Estado.GetByIdPais(IdPais);

            return Json(result.Objects);
        }

        public JsonResult GetMunicipio(int IdEstado)
        {
            var result = BL.Municipio.GetByIdEstado(IdEstado);

            return Json(result.Objects);
        }

        public JsonResult GetColonia(int IdMunicipio)
        {
            var result = BL.Colonia.GetByIdMunicipio(IdMunicipio);

            return Json(result.Objects);

        }

       public static Byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }


        public JsonResult CambiarEstatus(int IdUsuario, bool status)
        {
            ML.Result result = BL.Usuario.UpdateEstatus(IdUsuario, status);

                return Json(result);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            //ML.Result result = BL.Usuario.GetByUserName(UserName);

            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Usuario/GetByUserName/" + UserName);

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if(resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        ML.Usuario usuarioItem = new ML.Usuario();

                        usuarioItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        result.Object = usuarioItem;
                        result.Correct = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;

                if(usuario.Password == Password)
                {
                    return Redirect("Home/Index");
                }
                else
                {
                    ViewBag.Message = "El UserName o contraseña es incorrecto..";
                    return PartialView("ModalLogin");
                }
            }
            else
            {
                ViewBag.Message = "El UserName o contraseña es incorrecto..";
                return PartialView("ModalLogin");
            }
            

            
        }


    }
}
