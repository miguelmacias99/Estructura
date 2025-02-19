namespace Plantilla.Infraestructura.Modelo.Correo
{
    public class EnvioCorreoDto
    {
        public List<string> Emails { get; private set; }
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public Dictionary<string, byte[]>? Adjuntos { get; set; }

        public EnvioCorreoDto(string correo)
        {
            Emails = [correo];
        }

        public EnvioCorreoDto(List<string> correos)
        {
            Emails = correos;
        }
    }
}