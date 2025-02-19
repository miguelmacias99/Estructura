using Cititrans.Auth.Infraestructura.Modelo.Token;

namespace Cititrans.Auth.Infraestructura.Services.Tokens
{
    public interface IJwtService
    {
        JwtDto GenerarToken(List<JwtClaimDto> claims);
    }
}