using System.ComponentModel.DataAnnotations;

namespace Plantilla.Dto.Modelo.Procesos.TaskItem
{
    public class TaskItemDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool EsCompletada { get; set; }
        public bool Activo { get; set; }
        public class CrearTaskItem
        {
            [Required(ErrorMessage = "El titulo es obligatorio")]
            public required string Titulo { get; set; }

            [Required(ErrorMessage = "La descripción es obligatorio")]
            public required string Descripcion { get; set; }
            public DateTime? FechaVencimiento { get; set; }
            public required bool EsCompletada { get; set; }
        }

        public class ActualizarTaskItem
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }

            [Required(ErrorMessage = "El titulo es obligatorio")]
            public required string Titulo { get; set; }

            [Required(ErrorMessage = "La descripción es obligatorio")]
            public required string Descripcion { get; set; }
            public DateTime? FechaVencimiento { get; set; }
            public required bool EsCompletada { get; set; }
        }

        public class EliminarTaskItem
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }
        }

        public class ConsultarTaskItem
        {
            [Required(ErrorMessage = "El id es obligatorio")]
            public long? Id { get; set; }
        }
    }
}
