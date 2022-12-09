using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/<UsuarioController>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Usuarios = result.Objects;   //guardamos la lista de objetos en una lista de usuarios 
                return Ok(result);
            }
            else
            {
                
                return NotFound();
            }
        }


        [HttpPost("GetAll/")]
        public IActionResult GetAll(string? nombre, string? apellidoPaterno, byte? idRol)
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.Nombre = (nombre == null) ? "" : nombre;
            usuario.ApellidoPaterno = (apellidoPaterno == null) ? "" : apellidoPaterno;
  
            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = (idRol == null) ? usuario.Rol.IdRol = 0 : idRol.Value;

            ML.Result result= BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        // GET api/<UsuarioController>/5
        [HttpGet("GetById/{idusuario}")]
        public IActionResult Get(int idusuario)
        {
            ML.Result result = BL.Usuario.GetById(idusuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
            
        }

        // POST api/<UsuarioController>
        [HttpPost("Add")]
        public IActionResult Post([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("Update/{idusuario}")]
        public IActionResult Put(int idusuario, [FromBody] ML.Usuario usuario)
        {
            usuario.IdUsuario = idusuario;
            ML.Result result = BL.Usuario.Update(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
            
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("Delete/{idUsuario}")]
        public IActionResult Delete(int idUsuario)
        {
            if (idUsuario > 0)
            {
                ML.Result result = BL.Usuario.Delete(idUsuario);

                if (result.Correct)
                {
                    return Ok(result);

                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }


           

        }

        [HttpGet("GetByUserName/{username}")]
        public IActionResult Login(string username)
        {
            ML.Result result = BL.Usuario.GetByUserName(username);

            if(result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
