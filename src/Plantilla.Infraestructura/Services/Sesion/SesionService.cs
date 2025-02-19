using System.Security.Claims;
using Plantilla.Infraestructura.Constantes;
using Microsoft.AspNetCore.Http;

namespace Plantilla.Infraestructura.Services.Sesion
{
    internal class SesionService(IHttpContextAccessor httpContextAccessor) : ISesionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string ObtenerUsuarioActual()
        {
            var usuario = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return usuario ?? UsuarioSistemaConstante.UsuarioRegistro;
        }

        public string ObtenerIpCliente()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return "No disponible";

            string? ip = context.Connection.RemoteIpAddress?.ToString();

            // Verificar si hay encabezados de proxy (para aplicaciones detrás de un proxy o balanceador de carga)
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                ip = forwardedHeader.Split(',')[0]; // Tomar la primera IP en caso de múltiples proxies
            }

            return ip ?? "No disponible";
        }
    }
}