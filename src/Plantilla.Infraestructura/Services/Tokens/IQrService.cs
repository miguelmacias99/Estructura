using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Modelo.Token;

namespace Plantilla.Infraestructura.Services.Tokens
{
    public interface IQrService
    {
        Task<RespuestaGenericaConsultaDto<QrAuthenticatorResultado>> GenerarCodigoQrAuthenticator(QrAuthenticatorData data, [CallerMemberName] string metodoInvoca = "");
    }
}