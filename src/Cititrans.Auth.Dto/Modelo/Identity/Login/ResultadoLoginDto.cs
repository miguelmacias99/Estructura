using Cititrans.Auth.Infraestructura.Modelo.Token;

namespace Cititrans.Auth.Dto.Modelo.Identity.Login
{
    public class ResultadoLoginDto
    {
        public bool ValidarFactorAutenticacion { get; set; }
        public JwtDto? Jwt { get; set; }

        public ResultadoLoginDto()
        {
            ValidarFactorAutenticacion = false;
        }

        public ResultadoLoginDto(bool validarFactorAutenticacion)
        {
            ValidarFactorAutenticacion = validarFactorAutenticacion;
        }
    }
}