using Cititrans.Auth.Entidad.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.Entidad
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarContextos(this IServiceCollection services, IConfiguration configuracion)
        {
            services.AddDbContext<CititransDbContext>(options =>
                options.UseSqlServer(
                    configuracion.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("Cititrans.Auth.RepositorioEfCore")
                ));

            return services;
        }
    }
}