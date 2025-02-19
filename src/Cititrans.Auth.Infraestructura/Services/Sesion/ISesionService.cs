namespace Cititrans.Auth.Infraestructura.Services.Sesion
{
    public interface ISesionService
    {
        string ObtenerUsuarioActual();

        string ObtenerIpCliente();
    }
}