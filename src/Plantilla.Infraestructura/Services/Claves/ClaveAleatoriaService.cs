using Plantilla.Infraestructura.Modelo.Configuracion;
using Microsoft.Extensions.Options;
using PasswordGenerator;

namespace Plantilla.Infraestructura.Services.Claves
{
    internal class ClaveAleatoriaService(IOptions<ConfiguracionIdentity> options) : IClaveAleatoriaService
    {
        private readonly ConfiguracionIdentity configuracion = options.Value;

        public string GenerarClaveAleatorea()
        {
            var claveGenerador = new Password(configuracion.Password.RequiredLength);
            if (configuracion.Password.RequireUppercase) claveGenerador.IncludeUppercase();
            if (configuracion.Password.RequireLowercase) claveGenerador.IncludeLowercase();
            if (configuracion.Password.RequireDigit) claveGenerador.IncludeNumeric();
            if (configuracion.Password.RequireNonAlphanumeric) claveGenerador.IncludeSpecial();

            return claveGenerador.Next();
        }
    }
}