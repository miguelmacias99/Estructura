using Plantilla.Negocio.Identity;
using Plantilla.Negocio.Procesos.TaskItem;
using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.Negocio
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarNegocio(this IServiceCollection services)
        {
            services.AddNeIdentity();
            services.AddNeProcesos();

            return services;
        }
    }
}