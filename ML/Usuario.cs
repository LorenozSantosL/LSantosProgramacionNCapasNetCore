using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        //[Required]
        //[RegularExpression(@"/^[a-zA-Z\u00C0-\u00FF ]*$/",ErrorMessage = "Solo se permiten letras" )]
        public string Nombre { get; set; }

        //[Required]
        
        //[Display(Name = "Apellido Paterno ")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string? ApellidoMaterno { get; set; }

        //[Required]
        [Display(Name = "Fecha de Nacimiento")]
        public string FechaNacimiento { get; set; }

        //[Required]
        public string Sexo { get; set; }

        
        //[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        // ErrorMessage = "Correo Invalido.")]
        public string? Email { get; set; }

        //[Required]
        
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        //[Required]
        //[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$",
        //    ErrorMessage = "La contraseña debe de tener al menos 10 caracteres, una letra mayuscula, un caracter especial")]
        public string Password { get; set; }

        //[Required]
        //[MinLength(10, ErrorMessage = "El número debe de tener al menos 10 números")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Se debe de ingresar números enteros")]
        public string Telefono { get; set; }

        //[Required]
        //[MinLength(10, ErrorMessage = "El número debe de tener al menos 10 números")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Se debe de ingresar números enteros")]
        public string Celular { get; set; }

        //[Required]
        //[MinLength(18, ErrorMessage = "El CURP debe de tener al menos 18 caracteres")]
        public string CURP { get; set; }


        public bool Estatus { get; set; }
        public ML.Rol? Rol { get; set; }

         
         public string? Imagen  { get; set; }


         public List<object>? Usuarios { get; set; }

        public ML.Direccion? Direccion { get; set; }
    }
}