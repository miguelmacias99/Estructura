namespace Plantilla.Infraestructura.Modelo.Configuracion
{
    public class ConfiguracionJwt
    {
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public TimeSpan ClockSkew => TimeSpan.FromMinutes(ClockSkewInMinutes);
        public int ClockSkewInMinutes { get; set; }
    }
}