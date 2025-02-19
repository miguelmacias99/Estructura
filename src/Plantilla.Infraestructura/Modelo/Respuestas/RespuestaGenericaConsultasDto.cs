using System.Runtime.CompilerServices;

namespace Plantilla.Infraestructura.Modelo.Respuestas
{
    public class RespuestaGenericaConsultasDto<T> where T : class
    {
        public RespuestaGenericaDto Respuesta { get; set; } = null!;
        public List<T>? Resultados { get; set; }

        public RespuestaGenericaConsultasDto()
        {
        }

        public RespuestaGenericaConsultasDto(RespuestaGenericaDto respuesta)
        {
            this.Respuesta = respuesta;
        }

        public RespuestaGenericaConsultasDto(string codigo, string mensaje, [CallerMemberName] string metodoInvoca = "")
        {
            this.Respuesta = new RespuestaGenericaDto(codigo, mensaje, metodoInvoca);
        }
    }
}