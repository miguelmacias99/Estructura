using Cititrans.Auth.Entidad.Modelo.Identity;
using Cititrans.Auth.RepositorioEfCore.Servicios.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.RepositorioEfCore.Servicios
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection ConfigurarServiciosRepositorio(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender<AuthUser>, EmailSender>();
            services.ConfigurarIdentity();

            return services;
        }
    }
}