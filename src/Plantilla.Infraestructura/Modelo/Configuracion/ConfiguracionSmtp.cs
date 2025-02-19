namespace Plantilla.Infraestructura.Modelo.Configuracion
{
    public class ConfiguracionSmtp
    {
        public string Host { get; set; } = string.Empty;
        public int Puerto { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Desde { get; set; } = string.Empty;
    }
}