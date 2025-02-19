using Cititrans.Auth.Negocio.Identity.Login;
using Cititrans.Auth.Negocio.Identity.Rol;
using Cititrans.Auth.Negocio.Identity.Usuario;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.Negocio.Identity
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