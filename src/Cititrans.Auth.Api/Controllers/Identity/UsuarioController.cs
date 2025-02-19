using System.Net;
using Cititrans.Auth.Infraestructura.Constantes;
using Cititrans.Auth.Infraestructura.Services.Razor;
using Cititrans.Auth.Infraestructura.Utilidades.Logger;
using Cititrans.Auth.Negocio.Identity.Usuario;
using Microsoft.AspNetCore.Mvc;
using static Cititrans.Auth.Dto.Modelo.Identity.Usuario.UsuarioDto;

namespace Cititrans.Auth.Api.Controllers.Identity
{
    [ApiController]
    [Route("api/v1")]
    public class UsuarioController(INeUsuario neUsuario, IHtmlRenderService renderService) : ControllerBase
    {
        [HttpPost("consultar-usuario-id")]
        public async Task<IActionResult> ConsultarUsuario(ConsultarUsuario consultar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neUsuario.ConsultarPorId(consultar);

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

        [HttpPost("consultar-usuarios")]
        public async Task<IActionResult> ConsultarUsuarios(ConsultarUsuarios consultar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neUsuario.ConsultarTodos(consultar);

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

        [HttpPost("actualizar-usuario")]
        public async Task<IActionResult> ActualizarUsuario(ActualizarUsuario actualizar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.Actualizar(actualizar);

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

        [HttpPost("desbloquear-usuario")]
        public async Task<IActionResult> DesbloquearUsuario(DesbloquearUsuario desbloquear)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.Desbloquear(desbloquear);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, desbloquear);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpPost("eliminar-usuario")]
        public async Task<IActionResult> EliminarUsuario(EliminarUsuario eliminar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.Eliminar(eliminar);

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

        [HttpPost("crear-usuario")]
        public async Task<IActionResult> CrearUsuario(CrearUsuario crear)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.Crear(crear);

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

        [HttpPost("enviar-correo-confirmacion-usuario")]
        public async Task<IActionResult> EnviarConfirmacionCorreo(EnviarConfirmacionCorreoUsuario enviar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.EnviarConfirmacionCorreo(enviar);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, enviar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpGet("confirmar-correo-usuario")]
        public async Task<IActionResult> ValidarConfirmacionCorreo(long id, string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.ValidarConfirmacionCorreo(new()
                {
                    Id = id,
                    Token = token,
                });

                var htmlContent = respuesta.EsExitosa
                    ? await renderService.RenderizarVistaAsync("Views/Usuario/ConfirmacionCorreoExito.cshtml")
                    : await renderService.RenderizarVistaAsync("Views/Usuario/ConfirmacionCorreoError.cshtml");

                return Content(htmlContent, "text/html");
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, new
                {
                    id,
                    token,
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpPost("cambiar-clave-usuario")]
        public async Task<IActionResult> CambiarClave(CambiarClaveUsuario cambiar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.CambiarClave(cambiar);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, cambiar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpPost("enviar-correo-reestablecer-clave-usuario")]
        public async Task<IActionResult> EnviarCorreoClaveOlvidada(EnviarCorreoClaveOlvidadaUsuario enviar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.EnviarCorreoClaveOlvidada(enviar);

                return respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, respuesta)
                    : Ok(respuesta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, enviar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpGet("reestablecer-clave-usuario")]
        public async Task<IActionResult> ReestablecerClaveOlvidada(long id, string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var respuesta = await neUsuario.ReestablecerClaveOlvidada(new()
                {
                    Id = id,
                    Token = token,
                });

                var htmlContent = respuesta.Respuesta.EsExitosa
                    ? await renderService.RenderizarVistaModelAsync("Views/Usuario/ConfirmacionReestablecerCredenciales.cshtml", respuesta.Resultado)
                    : await renderService.RenderizarVistaAsync("Views/Usuario/ConfirmacionCorreoError.cshtml");

                return Content(htmlContent, "text/html");
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, new
                {
                    id,
                    token,
                });
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }
    }
}