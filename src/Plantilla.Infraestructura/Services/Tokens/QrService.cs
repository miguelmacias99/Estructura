using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Modelo.Configuracion;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Modelo.Token;
using Plantilla.Infraestructura.Utilidades.Logger;
using Microsoft.Extensions.Options;
using QRCoder;

namespace Plantilla.Infraestructura.Services.Tokens
{
    internal class QrService(IOptions<ConfiguracionQrCode> options) : IQrService
    {
        private readonly ConfiguracionQrCode configuracionQrCode = options.Value;

        public async Task<RespuestaGenericaConsultaDto<QrAuthenticatorResultado>> GenerarCodigoQrAuthenticator(QrAuthenticatorData data, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                var uriQr = $"otpauth://totp/{Uri.EscapeDataString(configuracionQrCode.NombreApp)}" +
                    $":{Uri.EscapeDataString(data.Correo)}?secret={data.AuthenticatorKey}&issuer={Uri.EscapeDataString(configuracionQrCode.NombreApp)}";
                var codigoQrBytes = GenerarCodigoQr(uriQr);

                await Task.CompletedTask;

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = new()
                    {
                        Uri = uriQr,
                        Base64 = Convert.ToBase64String(codigoQrBytes),
                        Bytes = codigoQrBytes,
                    },
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, data, metodoInvoca);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        private static byte[] GenerarCodigoQr(string texto)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}