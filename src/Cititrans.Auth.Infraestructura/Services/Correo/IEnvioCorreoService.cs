using System.Runtime.CompilerServices;
using Cititrans.Auth.Infraestructura.Modelo.Correo;
using Cititrans.Auth.Infraestructura.Modelo.Respuestas;

namespace Cititrans.Auth.Infraestructura.Services.Correo
{
    public interface IEnvioCorreoService
    {
        Task<RespuestaGenericaDto> EnviarCorreo(EnvioCorreoDto correo, [CallerMemberName] string metodoInvoca = "");

        Task<RespuestaGenericaDto> EnviarCorreos(List<EnvioCorreoDto> correos, [CallerMemberName] string metodoInvoca = "");
    }
}