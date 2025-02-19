namespace Plantilla.Infraestructura.Modelo.Token
{
    public class JwtClaimDto(string tipo, string valor)
    {
        public string Tipo { get; } = tipo;
        public string Valor { get; } = valor;
    }
}