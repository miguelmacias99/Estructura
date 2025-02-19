using System.ComponentModel.DataAnnotations;

namespace Plantilla.Dto.Modelo.Identity.Login
{
    public class LoginFactorAutenticacionDto
    {
        [Required(ErrorMessage = "Usuario es obligatorio")]
        public string? Usuario { get; set; }

        [Required(ErrorMessage = "Código doble factor autenticacion es obligatorio")]
        public string? CodigoFactorAutenticacion { get; set; }
    }
}