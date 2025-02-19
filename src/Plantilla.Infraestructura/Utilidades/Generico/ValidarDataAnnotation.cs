using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Utilidades.Logger;

namespace Plantilla.Infraestructura.Utilidades.Generico
{
    public static class ValidarDataAnnotation
    {
        public static (RespuestaGenericaDto respuesta, bool esValido) Validar<T>(T? modelo, [CallerMemberName] string metodoInvoca = "")
        {
            return ValidarPrivado<T>(modelo, metodoInvoca);
        }

        private static (RespuestaGenericaDto respuesta, bool esValido) ValidarPrivado<T>(T? modelo, [CallerMemberName] string metodoInvoca = "")
        {
            if (modelo is null)
                return (RespuestaGenericaDto.ErrorComun("La solicitud al servicio es inválida"), false);

            var context = new ValidationContext(modelo);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(modelo, context, validationResults, true))
            {
                var errores = validationResults
                    .Select(e => $"Error en el campo {e.MemberNames.FirstOrDefault()}: {e.ErrorMessage}")
                    .ToList();

                LogUtils.LogWarning(string.Join(",", errores), metodoInvoca);
                return (RespuestaGenericaDto.ErrorComun(string.Join(",", errores), metodoInvoca), false);
            }

            return (RespuestaGenericaDto.ExitoComun(), true);
        }
    }
}