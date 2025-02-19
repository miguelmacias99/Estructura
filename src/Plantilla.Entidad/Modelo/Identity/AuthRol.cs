using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plantilla.Infraestructura.Constantes;
using Microsoft.AspNetCore.Identity;

namespace Plantilla.Entidad.Modelo.Identity
{
    [Table(nameof(AuthRol), Schema = "IDE")]
    public class AuthRol : IdentityRole<long>
    {
        [Required]
        public bool Activo { get; set; }

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
    }
}