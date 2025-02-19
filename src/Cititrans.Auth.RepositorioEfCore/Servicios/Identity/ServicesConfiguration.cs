using Cititrans.Auth.Entidad.Interfaz.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.RepositorioEfCore.Servicios.Identity
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