namespace Plantilla.Infraestructura.Services.Sesion
{
    public interface ISesionService
    {
        string ObtenerUsuarioActual();

        string ObtenerIpCliente();
    }
}