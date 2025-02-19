using System.Net;
using Plantilla.Infraestructura.Constantes;
using Plantilla.Infraestructura.Utilidades.Logger;
using Plantilla.Negocio.Procesos.TaskItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Plantilla.Dto.Modelo.Procesos.TaskItem.TaskItemDto;

namespace Plantilla.Api.Controllers.Procesos
{
    [ApiController]
    [Route("api/v1")]
    public class TaskItemController(INeTaskItem neTaskItem) : ControllerBase
    {
        private readonly INeTaskItem neTaskItem = neTaskItem;

        [Authorize]
        [HttpPost("consultar-task-item-id")]
        public async Task<IActionResult> ConsultarTaskItem(ConsultarTaskItem consultar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neTaskItem.ConsultarPorId(consultar);

                return consulta.Respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, consulta)
                    : Ok(consulta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, consultar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [Authorize]
        [HttpPost("consultar-task-items")]
        public async Task<IActionResult> ConsultarTaskItemes()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neTaskItem.ConsultarTodos();

                return consulta.Respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, consulta)
                    : Ok(consulta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [Authorize]
        [HttpPost("actualizar-task-item")]
        public async Task<IActionResult> ActualizarTaskItem(ActualizarTaskItem actualizar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neTaskItem.Actualizar(actualizar);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, actualizar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [Authorize]
        [HttpPost("eliminar-task-item")]
        public async Task<IActionResult> EliminarTaskItem(EliminarTaskItem eliminar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neTaskItem.Eliminar(eliminar);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, eliminar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [Authorize]
        [HttpPost("crear-task-item")]
        public async Task<IActionResult> CrearTaskItem(CrearTaskItem crear)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neTaskItem.Crear(crear);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, crear);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }
    }
}