using Plantilla.Negocio.Identity.Login;
using Plantilla.Negocio.Identity.Rol;
using Plantilla.Negocio.Identity.Usuario;
using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.Negocio.Identity
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddNeIdentity(this IServiceCollection services)
        {
            services.AddScoped<INeRol, NeRol>();
            services.AddScoped<INeUsuario, NeUsuario>();
            services.AddScoped<INeLogin, NeLogin>();

            return services;
        }
    }
}