using System.ComponentModel.DataAnnotations;
using Plantilla.Infraestructura.Modelo.Consulta;

namespace Plantilla.Dto.Modelo.Identity.Usuario
{
    public class UsuarioDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsSuperUser { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool Activo { get; set; }
        public TwoFactorTypeDto Tipo2FA { get; set; }
        public string UsuarioRegistro { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string? IpRegistro { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? IpModificacion { get; set; }

        public class EnviarConfirmacionCorreoUsuario
        {
            [Required(ErrorMessage = "Email es obligatorio")]
            public string? Email { get; set; }
        }

        public class ValidarConfirmacionCorreoUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "El token es obligatorio")]
            public string? Token { get; set; }
        }

        public class ConsultarUsuarios
        {
            [Required(ErrorMessage = "Paginación es Obligatoria")]
            public Paginacion Paginacion { get; set; } = null!;
        }

        public class ConsultarUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }
        }

        public class EliminarUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }
        }

        public class CrearUsuario
        {
            [Required(ErrorMessage = "Nombre es obligatorio")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "Apellido es obligatorio")]
            public string? LastName { get; set; }

            [Required(ErrorMessage = "IsSuperUser es obligatorio")]
            public bool IsSuperUser { get; set; }

            [Required(ErrorMessage = "Email es obligatorio")]
            [EmailAddress(ErrorMessage = "Email no tiene un formato válido")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "Clave es obligatorio")]
            public string? PasswordHash { get; set; }

            public string? PhoneNumber { get; set; }

            [Required(ErrorMessage = "Tipo2FA es obligatorio")]
            public TwoFactorTypeDto Tipo2FA { get; set; }
        }

        public class ActualizarUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "Nombre es obligatorio")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "Apellido es obligatorio")]
            public string? LastName { get; set; }

            [Required(ErrorMessage = "IsSuperUser es obligatorio")]
            public bool IsSuperUser { get; set; }

            [Required(ErrorMessage = "Clave es obligatorio")]
            public string? PhoneNumber { get; set; }

            [Required(ErrorMessage = "Tipo2FA es obligatorio")]
            public TwoFactorTypeDto Tipo2FA { get; set; }
        }

        public class CambiarClaveUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "Clave actual es obligatorio")]
            public string? CurrentPasswordHash { get; set; }

            [Required(ErrorMessage = "Clave nueva es obligatorio")]
            public string? NewPasswordHash { get; set; }
        }

        public class EnviarCorreoClaveOlvidadaUsuario
        {
            [Required(ErrorMessage = "Email es obligatorio")]
            public string? Email { get; set; }
        }

        public class ReestablecerClaveUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "Token es obligatorio")]
            public string? Token { get; set; }
        }

        public class DesbloquearUsuario
        {
            [Required(ErrorMessage = "Id es obligatorio")]
            public long? Id { get; set; }
        }
    }
}