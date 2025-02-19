using Cititrans.Auth.Infraestructura.Services.Claves;
using Cititrans.Auth.Infraestructura.Services.Correo;
using Cititrans.Auth.Infraestructura.Services.Encriptacion;
using Cititrans.Auth.Infraestructura.Services.Razor;
using Cititrans.Auth.Infraestructura.Services.Redis;
using Cititrans.Auth.Infraestructura.Services.Sesion;
using Cititrans.Auth.Infraestructura.Services.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.Infraestructura.Services
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