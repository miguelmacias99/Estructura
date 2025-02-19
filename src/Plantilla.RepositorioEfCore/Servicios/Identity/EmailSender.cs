using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Infraestructura.Services.Correo;
using Plantilla.Infraestructura.Utilidades.Logger;
using Microsoft.AspNetCore.Identity;

namespace Plantilla.RepositorioEfCore.Servicios.Identity
{
    public class EmailSender(IEnvioCorreoService envioCorreo) : IEmailSender<AuthUser>
    {
        private readonly IEnvioCorreoService _envioCorreo = envioCorreo;

        public async Task SendConfirmationLinkAsync(AuthUser user, string email, string confirmationLink)
        {
            try
            {
                var respuesta = await _envioCorreo.EnviarCorreo(new(email)
                {
                    Asunto = "Confirmación de Cuenta",
                    Cuerpo = $"Confirma tu registro desde el siguiente enlace: \n {confirmationLink}",
                });

                if (!respuesta.EsExitosa)
                {
                    throw new InvalidOperationException(respuesta.Mensaje);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, new
                {
                    user,
                    email,
                    confirmationLink
                });
                throw;
            }
        }

        public async Task SendPasswordResetCodeAsync(AuthUser user, string email, string resetCode)
        {
            try
            {
                var respuesta = await _envioCorreo.EnviarCorreo(new(email)
                {
                    Asunto = "Código para Reestablecer Contraseña",
                    Cuerpo = $"Utiliza el siguiente código para reestablecer tu contraseña: \n {resetCode}",
                });

                if (!respuesta.EsExitosa)
                {
                    throw new InvalidOperationException(respuesta.Mensaje);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, new
                {
                    user,
                    email,
                    resetCode
                });
                throw;
            }
        }

        public async Task SendPasswordResetLinkAsync(AuthUser user, string email, string resetLink)
        {
            try
            {
                var respuesta = await _envioCorreo.EnviarCorreo(new(email)
                {
                    Asunto = "Enlace para Reestablecer Contraseña",
                    Cuerpo = $"Utiliza el siguiente enlace para reestablecer tu contraseña: \n {resetLink}",
                });

                if (!respuesta.EsExitosa)
                {
                    throw new InvalidOperationException(respuesta.Mensaje);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, new
                {
                    user,
                    email,
                    resetLink
                });
                throw;
            }
        }
    }
}