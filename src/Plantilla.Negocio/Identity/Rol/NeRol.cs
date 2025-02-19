using Plantilla.Dto.Modelo.Identity.Rol;
using Plantilla.Entidad.Interfaz;
using Plantilla.Infraestructura.Extensiones.TipoDatos;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Services.Sesion;
using Plantilla.Infraestructura.Utilidades.Generico;
using Plantilla.Infraestructura.Utilidades.Logger;
using Plantilla.Infraestructura.Utilidades.Mapeador;
using static Plantilla.Dto.Modelo.Identity.Rol.RolDto;

namespace Plantilla.Negocio.Identity.Rol
{
    internal class NeRol(IUnidadTrabajo unidadTrabajo, ISesionService sesionService) : INeRol
    {
        private readonly IUnidadTrabajo unidadTrabajo = unidadTrabajo;
        private readonly ISesionService sesionService = sesionService;

        public async Task<RespuestaGenericaDto> Actualizar(ActualizarRol actualizar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(actualizar);
                if (!esValido) return respuesta;

                // Procesamos
                var rol = await unidadTrabajo
                    .Roles
                    .ConsultarPorId(actualizar.Id!.Value);

                if (rol is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el rol a actualizar");

                rol.Name = actualizar.Name;
                rol.NormalizedName = actualizar.Name.ToNormalize();
                rol.UsuarioModificacion = sesionService.ObtenerUsuarioActual();
                rol.FechaModificacion = DateTime.Now;
                rol.IpModificacion = sesionService.ObtenerIpCliente();

                await unidadTrabajo.Roles.Actualizar(rol);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, actualizar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaConsultaDto<RolDto>> ConsultarPorId(ConsultarRol consultar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(consultar);
                if (!esValido) return new(respuesta);

                // Procesamos
                var rol = await unidadTrabajo
                    .Roles
                    .ConsultarPorId(consultar.Id!.Value);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = rol?.Mapear<RolDto>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, consultar);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaConsultasDto<RolDto>> ConsultarTodos()
        {
            try
            {
                // Procesamos
                var roles = await unidadTrabajo
                    .Roles
                    .ConsultarTodos();

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultados = roles?.Mapear<List<RolDto>>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaDto> Crear(CrearRol crear)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(crear);
                if (!esValido) return respuesta;

                var rol = await unidadTrabajo
                    .Roles
                    .ConsultarPorNombre(crear.Name!);
                if (rol is not null) return RespuestaGenericaDto.ErrorComun("Ya existe un rol registrado con el nombre indicado");

                await unidadTrabajo
                    .Roles
                    .Crear(new()
                    {
                        Activo = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Name = crear.Name,
                        NormalizedName = crear.Name.ToNormalize(),
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = sesionService.ObtenerUsuarioActual(),
                        IpRegistro = sesionService.ObtenerIpCliente(),
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = sesionService.ObtenerUsuarioActual(),
                        IpModificacion = sesionService.ObtenerIpCliente(),
                    });

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, crear);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> Eliminar(EliminarRol eliminar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(eliminar);
                if (!esValido) return respuesta;

                // Procesamos
                var rol = await unidadTrabajo
                    .Roles
                    .ConsultarPorId(eliminar.Id!.Value);

                if (rol is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el rol a eliminar");

                rol.Activo = false;
                rol.UsuarioModificacion = sesionService.ObtenerUsuarioActual();
                rol.FechaModificacion = DateTime.Now;
                rol.IpModificacion = sesionService.ObtenerIpCliente();

                await unidadTrabajo.Roles.Actualizar(rol);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, eliminar);
                return RespuestaGenericaDto.Excepcion();
            }
        }
    }
}