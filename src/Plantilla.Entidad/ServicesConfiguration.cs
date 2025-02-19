using Plantilla.Entidad.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.Entidad
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarContextos(this IServiceCollection services, IConfiguration configuracion)
        {
            services.AddDbContext<CititransDbContext>(options =>
                options.UseSqlServer(
                    configuracion.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("Plantilla.RepositorioEfCore")
                ));

            return services;
        }
    }
}