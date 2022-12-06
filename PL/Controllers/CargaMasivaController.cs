using Microsoft.AspNetCore.Mvc;

using System.IO;

using System.Text;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public CargaMasivaController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }




        [HttpGet]
        public ActionResult CargaMasiva()
        {

            ML.Result result = new ML.Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult CargaTXT()
        {
            ML.Result verifica = new ML.Result();

            bool hayErrores = false;
            IFormFile fileTxT = Request.Form.Files["archivoTXT"];




            if (fileTxT != null)
            {
                ML.Result result = new ML.Result();

                result.Objects = new List<object>();

                ML.Result resultError = new ML.Result();

                resultError.Objects = new List<object>();

                StreamReader Textfile = new StreamReader(fileTxT.OpenReadStream());
                string line;
                line = Textfile.ReadLine();
                while ((line = Textfile.ReadLine()) != null)
                {
                    string[] lines = line.Split('|');

                    ML.Usuario usuario = new ML.Usuario();

                    usuario.Nombre = lines[0];
                    usuario.ApellidoPaterno = lines[1];
                    usuario.ApellidoMaterno = lines[2];
                    usuario.FechaNacimiento = lines[3];
                    usuario.Sexo = lines[4];
                    usuario.Email = lines[5];
                    usuario.UserName = lines[6];
                    usuario.Password = lines[7];
                    usuario.Telefono = lines[8];
                    usuario.Celular = lines[9];
                    usuario.CURP = lines[10];

                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = byte.Parse(lines[11]);

                    usuario.Direccion = new ML.Direccion();

                    usuario.Direccion.Calle = lines[12];
                    usuario.Direccion.NumeroInterior = lines[13];
                    usuario.Direccion.NumeroExterior = lines[14];
                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = byte.Parse(lines[15]);

                    result = BL.Usuario.Add(usuario);

                    if (!result.Correct)
                    {
                        resultError.Objects.Add(result.Message);
                    }
                }

                if (resultError.Objects.Count > 0)
                {
                    string fileErrorCreaded = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";

                    if (System.IO.File.Exists(fileErrorCreaded))
                    {
                        System.IO.File.Delete(fileErrorCreaded);
                    }
                    
                    string fileError = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";


                    // string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");

                    using (StreamWriter writer = new StreamWriter(fileError))
                    {
                        foreach (string Ln in resultError.Objects)
                        {
                            writer.WriteLine(Ln);
                        }
                    }
                    ViewBag.Message = "Los usuarios no han sido agregados correctamente";
                    verifica.Correct = false;
                }
                else
                {
                    ViewBag.Message = "Se han registrado los usuarios";
                    verifica.Correct = true;
                }


                //string path = Server.MapPath("~/ErrorAdd.txt");
                //if (resultError.Objects.Count > 0)
                //{
                //    string path = Path.Combine
                //    TextWriter tw = new StreamWriter(fileError);
                //    foreach (string lineaError in resultError.Objects)
                //    {
                //        tw.WriteLine(lineaError);
                //    }
                //    tw.Close();

                //}

            }


                return PartialView("Modal", verifica);
        }


        [HttpPost]
        public ActionResult UsuarioCargaMasiva(ML.Usuario usuario)
        {
            ML.Result valida = new ML.Result();
            bool verifica = false;

            IFormFile archivo = Request.Form.Files["FileExcel"]; //obtenemos el arhivo
            //Session 

            if (HttpContext.Session.GetString("PathArchivo") == null) //verificamos si la sesion no esta abierta
            {
                //validación del excel

                if(archivo.Length > 0)
                {
                    string fileName = Path.GetFileName(archivo.FileName);   //obtenemos el nombre dela archivo
                    string folderPath = _configuration["PathFolder:value"]; //obtenemos la direccion del folder
                    string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower(); //obtenemos la extension
                    string extensionTipo = _configuration["TipoExcel"]; //obtenemos el tipo de archivo permitido

                    if(extensionArchivo == extensionTipo)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"; //creamos el path para el nuevo archivo
                    
                        if(!System.IO.File.Exists(filePath)) //validamos que el path exista
                        {
                            using(FileStream stream = new FileStream(filePath, FileMode.Create)) // creamos el archivo
                            {
                                archivo.CopyTo(stream);  //creamos copia del archivo
                            }
                            //se leerá el método 
                            string conecionString = _configuration["ConnectionStringExcel:Value"] + filePath;

                            ML.Result resultConvertExcel = BL.Usuario.ConvertirExcelToDataTable(conecionString);

                            if(resultConvertExcel.Correct)
                            {
                                ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultConvertExcel.Objects);

                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);

                                }

                                return View("CargaMasiva", resultValidacion);
                            
                            }
                            else
                            {
                               
                                ViewBag.Message = "Ocurrio un error al leer el archivo";
                                return View("Modal", verifica);
                            }

                        }
                    }

                }


            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:Value"] + rutaArchivoExcel;
                

                ML.Result resultData = BL.Usuario.ConvertirExcelToDataTable(connectionString);

                if(resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {
                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);

                        if(!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se inserti ek usuario con nombre: " + usuarioItem.Nombre + "Error: " + resultAdd.Message);
                        }

                        
                    }

                    if (resultErrores.Objects.Count > 0)
                    {
                        //string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");
                        string fileError = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";

                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string Ln in resultErrores.Objects)
                            {
                                writer.WriteLine(Ln);
                            }
                        }
                        ViewBag.Message = "Los usuarios no han sido agregados correctamente";
                        valida.Correct = false;
                    }
                    else
                    {
                        valida.Correct = true;
                        ViewBag.Message = "Se han registrado los usuarios";
                    }
                }



            }
            return PartialView("Modal", valida);
        }
    }
}
