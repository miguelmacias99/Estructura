using Cititrans.Auth.Negocio.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.Negocio
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarNegocio(this IServiceCollection services)
        {
            services.AddNeIdentity();

            return services;
        }
    }
}