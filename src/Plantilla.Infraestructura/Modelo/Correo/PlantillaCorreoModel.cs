namespace Plantilla.Infraestructura.Modelo.Correo
{
    public class PlantillaCorreoModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? LinkAccion { get; set; }
        public string? Codigo { get; set; }
    }
}