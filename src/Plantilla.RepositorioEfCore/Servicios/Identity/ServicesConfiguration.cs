using Plantilla.Entidad.Interfaz.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.RepositorioEfCore.Servicios.Identity
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection ConfigurarIdentity(this IServiceCollection services)
        {
            services.AddScoped<IRolRepositorio, RolRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

            return services;
        }
    }
}