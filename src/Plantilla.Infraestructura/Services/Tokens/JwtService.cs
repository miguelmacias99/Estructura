using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Plantilla.Infraestructura.Modelo.Configuracion;
using Plantilla.Infraestructura.Modelo.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Plantilla.Infraestructura.Services.Tokens
{
    public class JwtService(IOptions<ConfiguracionJwt> options) : IJwtService
    {
        private readonly ConfiguracionJwt configuracionJwt = options.Value;

        public JwtDto GenerarToken(List<JwtClaimDto> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracionJwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(configuracionJwt.TokenValidityInMinutes);
            var issuedAt = DateTime.UtcNow;
            var jti = Guid.NewGuid().ToString(); // Genera un identificador único para el token

            // Claims básicos del JWT
            var claimsJwt = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, jti),  // ID único del token
                new (JwtRegisteredClaimNames.Iat, new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),  // Fecha de emisión
                new (JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),  // Fecha de expiración
                new (JwtRegisteredClaimNames.Iss, configuracionJwt.ValidIssuer),  // Emisor del token
                new (JwtRegisteredClaimNames.Aud, configuracionJwt.ValidAudience)  // Audiencia del token
            };

            // Agregar los claims personalizados que recibe la función
            claimsJwt.AddRange(claims.Select(e => new Claim(e.Tipo!, e.Valor)));

            var token = new JwtSecurityToken(
                issuer: configuracionJwt.ValidIssuer,
                audience: configuracionJwt.ValidAudience,
                claims: claimsJwt,
                expires: expiration,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            return new()
            {
                AccessToken = accessToken,
                Expiration = expiration,
                RefreshToken = refreshToken
            };
        }
    }
}