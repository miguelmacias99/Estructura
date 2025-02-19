namespace Plantilla.Infraestructura.Services.Encriptacion
{
    public interface IEncriptarTextoService
    {
        public string Encriptar(string texto);

        public string Desencriptar(string texto);
    }
}