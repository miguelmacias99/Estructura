using System.Runtime.CompilerServices;

namespace Plantilla.Infraestructura.Modelo.Respuestas
{
    public class RespuestaGenericaConsultaDto<T> where T : class
    {
        public RespuestaGenericaDto Respuesta { get; set; } = null!;
        public T? Resultado { get; set; }

        public RespuestaGenericaConsultaDto()
        {
        }

        public RespuestaGenericaConsultaDto(RespuestaGenericaDto respuesta)
        {
            this.Respuesta = respuesta;
        }

        public RespuestaGenericaConsultaDto(string codigo, string mensaje, [CallerMemberName] string metodoInvoca = "")
        {
            this.Respuesta = new RespuestaGenericaDto(codigo, mensaje, metodoInvoca);
        }
    }
}