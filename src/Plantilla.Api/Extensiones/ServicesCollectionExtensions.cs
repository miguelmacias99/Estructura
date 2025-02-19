using System.Text;
using Plantilla.Entidad.Contextos;
using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Infraestructura.Constantes;
using Plantilla.Infraestructura.Modelo.Configuracion;
using Plantilla.Infraestructura.Services;
using Plantilla.Infraestructura.SobreEscritura;
using Plantilla.RepositorioEfCore.CargaInicial;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;

namespace Plantilla.Api.Extensiones
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection ConfigurarSerilog(
            this IServiceCollection servicesollection, WebApplicationBuilder builder)
        {
            var urlSeq = builder.Configuration["SeqUrl"]!;

            var logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.LifeTime", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.EntityFrameWork.Database", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("NombreAplicacion", InformacionApi.Nombre)
                .Enrich.WithCorrelationId()
                .WriteTo.Console(LogEventLevel.Information)
                .WriteTo.Async(s => s.Seq(urlSeq));

            Log.Logger = logger.CreateLogger();
            builder.Host.UseSerilog(Log.Logger);
            servicesollection.AddSingleton(Log.Logger);

            var messageTemplate = $"{InformacionApi.Nombre} Iniciada!!";
            Log.Logger.Information(messageTemplate);

            return servicesollection;
        }

        public static IServiceCollection ConfigurarIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var configuracionIdentity = configuration.GetSection("ConfiguracionIdentity").Get<ConfiguracionIdentity>()
                ?? throw new ArgumentNullException(nameof(configuration));

            services.AddIdentity<AuthUser, AuthRol>(opts =>
            {
                // configuraciones de login
                opts.SignIn.RequireConfirmedEmail = configuracionIdentity.SignIn.RequireConfirmedEmail; // Validar funcionalidad
                opts.User.RequireUniqueEmail = configuracionIdentity.User.RequireUniqueEmail;

                // Configuracion de clave
                opts.Password.RequireDigit = configuracionIdentity.Password.RequireDigit;
                opts.Password.RequireLowercase = configuracionIdentity.Password.RequireLowercase;
                opts.Password.RequireUppercase = configuracionIdentity.Password.RequireUppercase;
                opts.Password.RequireNonAlphanumeric = configuracionIdentity.Password.RequireNonAlphanumeric;
                opts.Password.RequiredLength = configuracionIdentity.Password.RequiredLength;

                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configuracionIdentity.Lockout.DefaultLockoutTimeSpanMinutes); // Tiempo de bloqueo (5 min)
                opts.Lockout.MaxFailedAccessAttempts = configuracionIdentity.Lockout.MaxFailedAccessAttempts; // Bloqueo tras 3 intentos fallidos
                opts.Lockout.AllowedForNewUsers = configuracionIdentity.Lockout.AllowedForNewUsers; // Aplicar bloqueo a nuevos usuarios

                // Configurar la caducidad de los tokens
                opts.Tokens.EmailConfirmationTokenProvider = configuracionIdentity.Tokens.EmailConfirmationTokenProvider;
                opts.Tokens.PasswordResetTokenProvider = configuracionIdentity.Tokens.PasswordResetTokenProvider;
            })
            .AddEntityFrameworkStores<CititransDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<IdentityMensajesEspanol>();

            // Configurar la caducidad de los tokens
            services.Configure<DataProtectionTokenProviderOptions>(opts =>
            {
                opts.TokenLifespan = TimeSpan.FromMinutes(configuracionIdentity.TokenLifespanMinutes);
            });

            return services;
        }

        public static IServiceCollection ConfigurarAutenticacionAutorizacion(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JWT").Get<ConfiguracionJwt>()
                ?? throw new ArgumentNullException(nameof(configuration));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = jwtConfig.ValidateIssuer,
                    ValidateAudience = jwtConfig.ValidateAudience,
                    ValidateLifetime = jwtConfig.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                    ClockSkew = jwtConfig.ClockSkew,

                    ValidAudiences = jwtConfig.ValidAudience.Split(","),
                    ValidIssuer = jwtConfig.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                };
            });

            services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));

            return services;
        }

        public static IServiceCollection ConfigurarRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var habilitarRedis = configuration.GetValue<bool>("Redis:Habilitar")!;
            if (habilitarRedis)
            {
                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(configuration.GetValue<string>("Redis:Url")!));

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetValue<string>("Redis:Url");
                });

                services.AddDistributedMemoryCache();
                services.ConfigurarServiciosRedis();
            }

            return services;
        }

        public static IServiceCollection ConfigurarSesion(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IApplicationBuilder AgregarOperacionesApi(this IApplicationBuilder application, string ambiente)
        {
            application.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async delegate (HttpContext context)
                {
                    await context.Response.WriteAsync(InformacionApi.Nombre + " " + ambiente + " Trabajando!!");
                });
            });
            return application;
        }

        public static async Task EjecutarCargaInicial(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            await IdentitySeeder.SeedUsersAndRolesAsync(services);
        }
        public static async Task EjecutarMigracionAlIniciar(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<CititransDbContext>();
                await context.Database.MigrateAsync(); // Aplica las migraciones pendientes
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error al aplicar migraciones de la base de datos.");
            }
        }
        public static IServiceCollection ConfigurarClasesDesdeAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfiguracionSmtp>(configuration.GetSection("ConfiguracionSmtp"));
            services.Configure<UrlIdentity>(configuration.GetSection("UrlIdentity"));
            services.Configure<ConfiguracionIdentity>(configuration.GetSection("ConfiguracionIdentity"));
            services.Configure<ConfiguracionJwt>(configuration.GetSection("JWT"));
            services.Configure<ConfiguracionQrCode>(configuration.GetSection("ConfiguracionQrCode"));

            return services;
        }
    }
}