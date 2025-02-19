using Plantilla.Infraestructura.Modelo.Token;

namespace Plantilla.Dto.Modelo.Identity.Login
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