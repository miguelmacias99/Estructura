using Plantilla.Dto.Modelo.Procesos.TaskItem;
using Plantilla.Infraestructura.Modelo.Respuestas;
using static Plantilla.Dto.Modelo.Procesos.TaskItem.TaskItemDto;

namespace Plantilla.Negocio.Procesos.TaskItem
{
    public interface INeTaskItem
    {
        Task<RespuestaGenericaConsultaDto<TaskItemDto>> ConsultarPorId(ConsultarTaskItem consultar);

        Task<RespuestaGenericaConsultasDto<TaskItemDto>> ConsultarTodos();

        Task<RespuestaGenericaDto> Crear(CrearTaskItem crear);

        Task<RespuestaGenericaDto> Actualizar(ActualizarTaskItem actualizar);

        Task<RespuestaGenericaDto> Eliminar(EliminarTaskItem eliminar);
    }
}
