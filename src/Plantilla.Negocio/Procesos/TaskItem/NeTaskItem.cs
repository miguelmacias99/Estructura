using Plantilla.Dto.Modelo.Identity.Rol;
using Plantilla.Dto.Modelo.Procesos.TaskItem;
using Plantilla.Entidad.Interfaz;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Services.Sesion;
using Plantilla.Infraestructura.Utilidades.Generico;
using Plantilla.Infraestructura.Utilidades.Logger;
using Plantilla.Infraestructura.Utilidades.Mapeador;

namespace Plantilla.Negocio.Procesos.TaskItem
{
    internal class NeTaskItem(IUnidadTrabajo unidadTrabajo) : INeTaskItem
    {
        public async Task<RespuestaGenericaDto> Actualizar(TaskItemDto.ActualizarTaskItem actualizar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(actualizar);
                if (!esValido) return respuesta;

                // Procesamos
                var entidad = await unidadTrabajo
                    .TaskItems
                    .ConsultarPorId(actualizar.Id!.Value);

                if (entidad is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el entidad a actualizar");

                entidad.Titulo = actualizar.Titulo;
                entidad.Descripcion = actualizar.Descripcion;
                entidad.FechaVencimiento = actualizar.FechaVencimiento;
                entidad.EsCompletada = actualizar.EsCompletada;

                await unidadTrabajo.TaskItems.Actualizar(entidad);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, actualizar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaConsultaDto<TaskItemDto>> ConsultarPorId(TaskItemDto.ConsultarTaskItem consultar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(consultar);
                if (!esValido) return new(respuesta);

                // Procesamos
                var entidad = await unidadTrabajo
                    .TaskItems
                    .ConsultarPorId(consultar.Id!.Value);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = entidad?.Mapear<TaskItemDto>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, consultar);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaConsultasDto<TaskItemDto>> ConsultarTodos()
        {
            try
            {
                // Procesamos
                var entidades = await unidadTrabajo
                    .TaskItems
                    .ConsultarTodos();

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultados = entidades?.Mapear<List<TaskItemDto>>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaDto> Crear(TaskItemDto.CrearTaskItem crear)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(crear);
                if (!esValido) return respuesta;

                await unidadTrabajo
                    .TaskItems
                    .Crear(new()
                    {
                        Descripcion = crear.Descripcion,
                        EsCompletada = crear.EsCompletada,
                        FechaCreacion = DateTime.Now,
                        Titulo = crear.Titulo,
                        FechaVencimiento = crear.FechaVencimiento,
                        Activo = true
                    });

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, crear);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> Eliminar(TaskItemDto.EliminarTaskItem eliminar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(eliminar);
                if (!esValido) return respuesta;

                // Procesamos
                var entidad = await unidadTrabajo
                    .TaskItems
                    .ConsultarPorId(eliminar.Id!.Value);

                if (entidad is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el entidad a eliminar");

                entidad.Activo = false;

                await unidadTrabajo.TaskItems.Actualizar(entidad);

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
