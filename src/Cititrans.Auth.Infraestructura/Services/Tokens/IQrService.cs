using System.Runtime.CompilerServices;
using Cititrans.Auth.Infraestructura.Modelo.Respuestas;
using Cititrans.Auth.Infraestructura.Modelo.Token;

namespace Cititrans.Auth.Infraestructura.Services.Tokens
{
    public interface IQrService
    {
        Task<RespuestaGenericaConsultaDto<QrAuthenticatorResultado>> GenerarCodigoQrAuthenticator(QrAuthenticatorData data, [CallerMemberName] string metodoInvoca = "");
    }
}