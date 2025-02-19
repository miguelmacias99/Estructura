using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Plantilla.Infraestructura.Extensiones.Sesion
{
    public static class SessionExtensions
    {
        public static void AgregarObjeto(this ISession session, string key, object value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public static T? RecuperarObjeto<T>(this ISession session, string key)
        {
            var isValid = session.TryGetValue(key, out var value);
            return isValid
                ? JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value))
                : default;
        }

        public static void LimpiarSesion(this ISession session)
        {
            foreach (var item in session.Keys)
            {
                session.Remove(item);
            }

            session.Clear();
        }
    }
}