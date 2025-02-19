using Plantilla.Infraestructura.Constantes;

namespace Plantilla.Api.Middleware
{
    public class LogMiddleware(
        RequestDelegate next,
        Serilog.ILogger logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly Serilog.ILogger _logger = logger;
        private const string m_Generico = "Generico";
        private const string m_ContentType = @"application/json";

        public const string PLANTILLA_MENSAJE_ERROR = "Error - {@NombreApi} HTTP {@RequestMethod} {@RequestPath} respondio {@StatusCode} en {@Elapsed:0.0000} ms";
        public const string PLANTILLA_MENSAJE_EVENTOS = "Trazabilidad - {@NombreApi} HTTP {@RequestMethod} - {@RequestPath} respondio el codigo de estado HTTP: {@StatusCode} en {@Elapsed:0.0000} ms con el usuario {@Usuario} el  Metodo invocado {@Metodo} desde la Ip Cliente {@IpCliente} los datos procesados son {@informacionEntrada} - Aplicacion {@Aplicacion} - TipoLog {@TipoLog} - FechaLog {@FechaLog}";
        public const string PLANTILLA_MENSAJE_AUDITORIA = "Auditoria - {@NombreApi}  Usuario {@Usuario} - {@Accion} respuesta: {@RespuestaServicio} desde la Ip Cliente {@IpCliente} los datos procesados son {@informacionEntrada} - Aplicacion {@Aplicacion} - TipoLog {@TipoLog} - FechaLog {@FechaLog}";

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            var tiempoInicio = DateTime.Now;

            try
            {
                httpContext.Request.EnableBuffering();

                using var reader = new StreamReader(httpContext.Request.Body);
                var json = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

                await _next(httpContext);

                await GuardaLogEventosAsync(httpContext, json, tiempoInicio);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionesAsync(httpContext, ex, tiempoInicio, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task ManejadorExcepcionesAsync(HttpContext context, Exception exception, DateTime tiempoInicio, int codigoEstado)
        {
            var tiempoFinal = DateTime.Now;
            var tiempoTotal = tiempoFinal - tiempoInicio;

            try
            {
                _logger.Error(
                   exception,
                   PLANTILLA_MENSAJE_ERROR,
                   InformacionApi.Nombre,
                   context.Request.Method,
                   context.Request.Path,
                   codigoEstado,
                   tiempoTotal.TotalMilliseconds);

                context.Response.Clear();
                context.Response.StatusCode = codigoEstado;
                context.Response.ContentType = m_ContentType;
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    PLANTILLA_MENSAJE_ERROR,
                    InformacionApi.Nombre,
                    context.Request.Method,
                    context.Request.Path,
                    codigoEstado,
                    tiempoTotal.TotalMilliseconds);

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = m_ContentType;
            }

            await Task.CompletedTask;
        }

        private async Task GuardaLogEventosAsync(HttpContext context,
            string? json = null, DateTime tiempoInicio = default)

        {
            try
            {
                var tiempoFinal = DateTime.Now;
                var tiempoTotal = tiempoFinal - tiempoInicio;
                var path = context.Request.Path;
                var infoData = (path.Value ?? string.Empty).Split('/');
                var metodo = infoData.LastOrDefault() ?? string.Empty;
                var ipCliente = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

                var usuario =
                    string.IsNullOrEmpty(context.User.Identity?.Name)
                        ? m_Generico
                        : context.User.Identity.Name;

                _logger.Information(
                    PLANTILLA_MENSAJE_EVENTOS,
                    InformacionApi.Nombre,
                    metodo,
                    context.Request.Path,
                    context.Response.StatusCode,
                    tiempoTotal.TotalMilliseconds,
                    usuario,
                    metodo,
                    ipCliente,
                    json,
                    InformacionApi.Nombre,
                    "Monitoreo",
                    DateTime.Now
                    );
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    PLANTILLA_MENSAJE_ERROR,
                    InformacionApi.Nombre,
                    context.Request.Method,
                    context.Request.Path,
                    605,
                    0);
            }

            await Task.CompletedTask;
        }
    }
}