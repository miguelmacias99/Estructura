using Plantilla.Entidad.Contextos;
using Plantilla.Entidad.Interfaz.Procesos;
using Plantilla.Entidad.Modelo.Procesos;
using Microsoft.EntityFrameworkCore;

namespace Plantilla.RepositorioEfCore.Servicios.Procesos
{
    public class TaskItemRepositorio(CititransDbContext context) : ITaskItemRepositorio
    {
        public async Task<TaskItem?> ConsultarPorId(long id)
        {
            return await context
                .TaskItems
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> ConsultarTodos()
        {
            return await context
                .TaskItems
                .ToListAsync();
        }

        public async Task<long> Crear(TaskItem taskItem)
        {
            await context.TaskItems.AddAsync(taskItem);
            await context.SaveChangesAsync();

            return taskItem.Id;
        }

        public async Task Actualizar(TaskItem taskItem)
        {
            context.Update(taskItem);
            await context.SaveChangesAsync();
        }
    }
}
