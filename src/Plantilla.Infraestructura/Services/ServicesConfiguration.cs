using Plantilla.Infraestructura.Services.Claves;
using Plantilla.Infraestructura.Services.Correo;
using Plantilla.Infraestructura.Services.Encriptacion;
using Plantilla.Infraestructura.Services.Razor;
using Plantilla.Infraestructura.Services.Redis;
using Plantilla.Infraestructura.Services.Sesion;
using Plantilla.Infraestructura.Services.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Plantilla.Infraestructura.Services
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigurarServiciosRedis(this IServiceCollection services)
        {
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection ConfigurarInfraestructura(this IServiceCollection services)
        {
            services.AddScoped<ISesionService, SesionService>();
            services.AddScoped<IEncriptarTextoService, EncriptarTextoService>();
            services.AddTransient<IEnvioCorreoService, EnvioCorreoService>();
            services.AddTransient<IHtmlRenderService, HtmlRenderService>();
            services.AddTransient<IClaveAleatoriaService, ClaveAleatoriaService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IQrService, QrService>();

            return services;
        }
    }
}