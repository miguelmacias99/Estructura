using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.Negocio.Procesos.TaskItem
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddNeProcesos(this IServiceCollection services)
        {
            services.AddScoped<INeTaskItem, NeTaskItem>();

            return services;
        }
    }
}