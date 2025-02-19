using Plantilla.Dto.Modelo.Identity.Rol;
using Plantilla.Infraestructura.Modelo.Respuestas;
using static Plantilla.Dto.Modelo.Identity.Rol.RolDto;

namespace Plantilla.Negocio.Identity.Rol
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