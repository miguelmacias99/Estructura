using System.Runtime.CompilerServices;
using Plantilla.Infraestructura.Constantes;
using Plantilla.Infraestructura.Utilidades.Logger;

namespace Plantilla.Infraestructura.Modelo.Respuestas
{
    public record RespuestaGenericaDto
    {
        public string Codigo { get; private set; } = null!;
        public string Mensaje { get; private set; } = null!;
        public bool EsExitosa => Codigo == Servidor.ExitoComun;
        public bool ExisteExcepcion => Codigo == Servidor.Excepcion;
        public RespuestaGenericaDto() { }

        public RespuestaGenericaDto(string codigo, string mensaje, [CallerMemberName] string metodoInvoca = "")
        {
            Codigo = codigo;
            Mensaje = mensaje;

            // Escribimos en el log de errores cuando exista un mensaje controlado
            if (!Servidor.CodigosNoLog.Contains(codigo))
            {
                LogUtils.LogWarning(mensaje, metodoInvoca);
            }
        }

        public static RespuestaGenericaDto ExitoComun(string mensaje = "Operación realizada con éxito")
        {
            return new RespuestaGenericaDto
            {
                Codigo = Servidor.ExitoComun,
                Mensaje = mensaje
            };
        }

        public static RespuestaGenericaDto ErrorComun(string mensaje = "Ha ocurrido un error", [CallerMemberName] string metodoInvoca = "")
        {
            // Escribimos en el log de errores cuando exista un mensaje controlado
            LogUtils.LogWarning(mensaje, metodoInvoca);

            return new RespuestaGenericaDto
            {
                Codigo = Servidor.ErrorComun,
                Mensaje = mensaje
            };
        }

        public static RespuestaGenericaDto Excepcion(string mensaje = "Ha ocurrido una excepción")
        {
            return new RespuestaGenericaDto
            {
                Codigo = Servidor.Excepcion,
                Mensaje = mensaje
            };
        }
    }
}