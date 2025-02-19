using Plantilla.Dto.Modelo.Identity.Usuario;
using Plantilla.Infraestructura.Modelo.Respuestas;
using static Plantilla.Dto.Modelo.Identity.Usuario.UsuarioDto;

namespace Plantilla.Negocio.Identity.Usuario
{
    public interface INeUsuario
    {
        Task<RespuestaGenericaConsultaDto<UsuarioDto>> ConsultarPorId(ConsultarUsuario consultar);

        Task<RespuestaGenericaConsultasDto<UsuarioDto>> ConsultarTodos(ConsultarUsuarios consultar);

        Task<RespuestaGenericaDto> Crear(CrearUsuario crear);

        Task<RespuestaGenericaDto> Actualizar(ActualizarUsuario actualizar);

        Task<RespuestaGenericaDto> Desbloquear(DesbloquearUsuario desbloquear);

        Task<RespuestaGenericaDto> Eliminar(EliminarUsuario eliminar);

        Task<RespuestaGenericaDto> EnviarConfirmacionCorreo(EnviarConfirmacionCorreoUsuario enviar);

        Task<RespuestaGenericaDto> ValidarConfirmacionCorreo(ValidarConfirmacionCorreoUsuario validar);

        Task<RespuestaGenericaDto> EnviarCorreoClaveOlvidada(EnviarCorreoClaveOlvidadaUsuario enviar);

        Task<RespuestaGenericaConsultaDto<ClaveReestablecidaUsuarioDto>> ReestablecerClaveOlvidada(ReestablecerClaveUsuario reestablecer);

        Task<RespuestaGenericaDto> CambiarClave(CambiarClaveUsuario cambiar);
    }
}