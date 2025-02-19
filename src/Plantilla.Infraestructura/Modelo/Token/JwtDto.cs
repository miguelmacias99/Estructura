namespace Plantilla.Infraestructura.Modelo.Token
{
    public class JwtDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}