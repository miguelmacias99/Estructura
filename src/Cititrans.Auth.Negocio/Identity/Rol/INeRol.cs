using Cititrans.Auth.Dto.Modelo.Identity.Rol;
using Cititrans.Auth.Infraestructura.Modelo.Respuestas;
using static Cititrans.Auth.Dto.Modelo.Identity.Rol.RolDto;

namespace Cititrans.Auth.Negocio.Identity.Rol
{
    public interface INeRol
    {
        Task<RespuestaGenericaConsultaDto<RolDto>> ConsultarPorId(ConsultarRol consultar);

        Task<RespuestaGenericaConsultasDto<RolDto>> ConsultarTodos();

        Task<RespuestaGenericaDto> Crear(CrearRol crear);

        Task<RespuestaGenericaDto> Actualizar(ActualizarRol actualizar);

        Task<RespuestaGenericaDto> Eliminar(EliminarRol eliminar);
    }
}