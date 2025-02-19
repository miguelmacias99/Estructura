using Plantilla.Dto.Modelo.Identity.Login;
using Plantilla.Infraestructura.Modelo.Respuestas;

namespace Plantilla.Negocio.Identity.Login
{
    public interface INeLogin
    {
        Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> IniciarSesion(LoginDto login);

        Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> ValidarFactorAutenticacion(LoginFactorAutenticacionDto login);

        Task<RespuestaGenericaConsultaDto<ResultadoQrDto>> GenerarCodigoQrAutenticathor(GenerarQrAutenticadorUsuarioDto generar);

        Task<RespuestaGenericaDto> CerrarSesion();
    }
}