using Plantilla.Entidad.Modelo.Procesos;

namespace Plantilla.Entidad.Interfaz.Procesos
{
    public interface ITaskItemRepositorio
    {
        Task<long> Crear(TaskItem taskItem);
        Task Actualizar(TaskItem taskItem);
        Task<TaskItem?> ConsultarPorId(long id);
        Task<IEnumerable<TaskItem>> ConsultarTodos();
    }
}
