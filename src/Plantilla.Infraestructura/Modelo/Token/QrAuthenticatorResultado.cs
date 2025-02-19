namespace Plantilla.Infraestructura.Modelo.Token
{
    public class QrAuthenticatorResultado
    {
        public string Base64 { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public byte[] Bytes { get; set; } = [];
    }
}