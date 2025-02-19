using System.Net;
using Cititrans.Auth.Dto.Modelo.Identity.Login;
using Cititrans.Auth.Infraestructura.Constantes;
using Cititrans.Auth.Infraestructura.Utilidades.Logger;
using Cititrans.Auth.Negocio.Identity.Login;
using Microsoft.AspNetCore.Mvc;

namespace Cititrans.Auth.Api.Controllers.Identity
{
    [ApiController]
    [Route("api/v1")]
    public class LoginController(INeLogin neLogin) : ControllerBase
    {
        private readonly INeLogin neLogin = neLogin;

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> InicioSesion(LoginDto login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neLogin.IniciarSesion(login);

                return consulta.Respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, consulta)
                    : Ok(consulta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, login);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpPost("validar-factor-autenticacion-inicio-sesion")]
        public async Task<IActionResult> ValidarFactorAutenticacion(LoginFactorAutenticacionDto validar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neLogin.ValidarFactorAutenticacion(validar);

                return consulta.Respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, consulta)
                    : Ok(consulta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, validar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }

        [HttpPost("generar-codigo-qr-autenticathor")]
        public async Task<IActionResult> GenerarCodigoQrAutenticathor(GenerarQrAutenticadorUsuarioDto generar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var consulta = await neLogin.GenerarCodigoQrAutenticathor(generar);

                return consulta.Respuesta.ExisteExcepcion
                    ? StatusCode((int)HttpStatusCode.InternalServerError, consulta)
                    : Ok(consulta);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, generar);
                return StatusCode((int)HttpStatusCode.InternalServerError, MensajeControladorConstante.HaOcurridoError);
            }
        }
    }
}