using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Modelo.Correo;
using Plantilla.Infraestructura.Modelo.Respuestas;

namespace Plantilla.Infraestructura.Services.Correo
{
    public interface IEnvioCorreoService
    {
        Task<RespuestaGenericaDto> EnviarCorreo(EnvioCorreoDto correo, [CallerMemberName] string metodoInvoca = "");

        Task<RespuestaGenericaDto> EnviarCorreos(List<EnvioCorreoDto> correos, [CallerMemberName] string metodoInvoca = "");
    }
}