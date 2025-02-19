using System.ComponentModel.DataAnnotations;

namespace Cititrans.Auth.Dto.Modelo.Identity.Login
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatorio")]
        public string? Clave { get; set; }

        public bool RecordarSesion { get; set; }
    }
}