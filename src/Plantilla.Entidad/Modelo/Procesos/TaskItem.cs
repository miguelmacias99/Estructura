using System.ComponentModel.DataAnnotations;
using Plantilla.Entidad.Modelo.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla.Entidad.Modelo.Procesos
{
    [Table(nameof(TaskItem), Schema = "TAR")]
    public class TaskItem
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public required string Titulo { get; set; }

        [MaxLength(500)]
        public required string Descripcion { get; set; }

        public required DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public required bool EsCompletada { get; set; }
        public required bool Activo { get; set; }
    }
}
