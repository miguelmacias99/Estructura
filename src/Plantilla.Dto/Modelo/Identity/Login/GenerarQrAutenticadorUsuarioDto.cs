using System.ComponentModel.DataAnnotations;

namespace Plantilla.Dto.Modelo.Identity.Login
{
    public class GenerarQrAutenticadorUsuarioDto
    {
        [Required(ErrorMessage = "Usuario es obligatorio")]
        public string? Usuario { get; set; }
    }
}