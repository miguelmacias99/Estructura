using System.ComponentModel.DataAnnotations;

namespace Plantilla.Dto.Modelo.Identity.Rol
{
    public class RolDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public bool Activo { get; set; }
        public string UsuarioRegistro { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string? IpRegistro { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? IpModificacion { get; set; }

        public class CrearRol
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string? Name { get; set; }
        }

        public class ActualizarRol
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string? Name { get; set; }
        }

        public class EliminarRol
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }
        }

        public class ConsultarRol
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }
        }
    }
}