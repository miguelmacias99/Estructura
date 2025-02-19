using Cititrans.Auth.Dto.Modelo.Identity.Usuario;
using Cititrans.Auth.Infraestructura.Modelo.Respuestas;
using static Cititrans.Auth.Dto.Modelo.Identity.Usuario.UsuarioDto;

namespace Cititrans.Auth.Negocio.Identity.Usuario
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