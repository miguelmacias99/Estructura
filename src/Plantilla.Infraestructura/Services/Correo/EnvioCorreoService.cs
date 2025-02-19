using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Modelo.Configuracion;
using Plantilla.Infraestructura.Modelo.Correo;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Utilidades.Logger;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Plantilla.Infraestructura.Services.Correo
{
    internal class EnvioCorreoService(IOptions<ConfiguracionSmtp> configuracion) : IEnvioCorreoService
    {
        private readonly ConfiguracionSmtp _configuracion = configuracion.Value;

        public async Task<RespuestaGenericaDto> EnviarCorreo(EnvioCorreoDto correo, [CallerMemberName] string metodoInvoca = "")
        {
            return await EnviarCorreoPrivado(correo, metodoInvoca);
        }

        public async Task<RespuestaGenericaDto> EnviarCorreos(List<EnvioCorreoDto> correos, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                List<Task<RespuestaGenericaDto>> tareas = [];
                foreach (var correo in correos)
                {
                    var tarea = EnviarCorreoPrivado(correo, metodoInvoca);
                    tareas.Add(tarea);
                }

                var tareasEjecutada = await Task.WhenAll(tareas);

                return tareasEjecutada.Any(e => !e.EsExitosa)
                    ? RespuestaGenericaDto.ErrorComun("Algunos correos no se enviaron correctamente, por favor revisar el log de eventos.")
                    : RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, correos, metodoInvoca);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        private async Task<RespuestaGenericaDto> EnviarCorreoPrivado(EnvioCorreoDto correo, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                var emailMessage = ObtenerMensaje(correo);

                using var client = new SmtpClient();
                await client.ConnectAsync(_configuracion.Host, _configuracion.Puerto, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuracion.Usuario, _configuracion.Clave);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, correo, metodoInvoca);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        private MimeMessage ObtenerMensaje(EnvioCorreoDto correo)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_configuracion.Desde, _configuracion.Desde));
            message.Subject = correo.Asunto;

            var bodyData = new Multipart("mixed")
            {
                new TextPart("html") { Text = correo.Cuerpo }
            };

            if (correo.Adjuntos != null)
            {
                foreach (var adjunto in correo.Adjuntos)
                {
                    bodyData.Add(new MimePart()
                    {
                        Content = new MimeContent(new MemoryStream(adjunto.Value)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = adjunto.Key
                    });
                }
            }

            message.Body = bodyData;

            foreach (var email in correo.Emails)
            {
                message.To.Add(new MailboxAddress(email, email));
            }

            return message;
        }
    }
}