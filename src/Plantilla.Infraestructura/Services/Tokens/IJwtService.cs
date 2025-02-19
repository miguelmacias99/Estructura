using Plantilla.Infraestructura.Modelo.Token;

namespace Plantilla.Infraestructura.Services.Tokens
{
    public interface IJwtService
    {
        JwtDto GenerarToken(List<JwtClaimDto> claims);
    }
}