using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plantilla.Infraestructura.Constantes;
using Microsoft.AspNetCore.Identity;

namespace Plantilla.Entidad.Modelo.Identity
{
    [Table(nameof(AuthUser), Schema = "IDE")]
    public class AuthUser : IdentityUser<long>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public bool IsSuperUser { get; set; }

        [Required]
        public bool Activo { get; set; }

        public string? TwoFASecret { get; set; }
        public TwoFactorType Tipo2FA { get; set; } = TwoFactorType.None;

        [Required]
        [StringLength(100)]
        public string UsuarioRegistro { get; set; } = UsuarioSistemaConstante.UsuarioRegistro;

        [Required]
        public DateTime FechaRegistro { get; set; }

        [StringLength(50)]
        public string? IpRegistro { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        [StringLength(100)]
        public DateTime? FechaModificacion { get; set; }

        [StringLength(50)]
        public string? IpModificacion { get; set; }

        #region Métodos adicionales

        public string ObtenerNombres()
        {
            return $"{FirstName} {FirstName}";
        }

        public bool Tiene2AFAuthenticatorEnabled()
        {
            return TwoFactorEnabled && Tipo2FA == TwoFactorType.Authenticator;
        }

        #endregion Métodos adicionales
    }
}