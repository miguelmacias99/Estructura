using System.Data;
using Cititrans.Auth.Entidad.Interfaz;
using Cititrans.Auth.RepositorioEfCore.Servicios;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.RepositorioEfCore
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarBaseDatos(this IServiceCollection services, string cadenaConexion)
        {
            services.AddScoped<IDbConnection>(db => new SqlConnection(cadenaConexion));
            services.AddScoped<IDbTransaction>(s =>
            {
                SqlConnection conn = s.GetRequiredService<SqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });
            return services;
        }

        public static IServiceCollection ConfigurarRepositorioSqlServer(this IServiceCollection services)
        {
            services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();
            services.ConfigurarServiciosRepositorio();

            return services;
        }
    }
}