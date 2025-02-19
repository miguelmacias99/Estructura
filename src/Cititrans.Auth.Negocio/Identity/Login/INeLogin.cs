using Cititrans.Auth.Dto.Modelo.Identity.Login;
using Cititrans.Auth.Infraestructura.Modelo.Respuestas;

namespace Cititrans.Auth.Negocio.Identity.Login
{
    public interface INeLogin
    {
        Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> IniciarSesion(LoginDto login);

        Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> ValidarFactorAutenticacion(LoginFactorAutenticacionDto login);

        Task<RespuestaGenericaConsultaDto<ResultadoQrDto>> GenerarCodigoQrAutenticathor(GenerarQrAutenticadorUsuarioDto generar);

        Task<RespuestaGenericaDto> CerrarSesion();
    }
}