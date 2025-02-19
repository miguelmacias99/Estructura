namespace Plantilla.Infraestructura.Extensiones.TipoDatos
{
    public static class StringExtensions
    {
        public static string ToNormalize(this string? texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            return texto.ToUpper().Trim();
        }
    }
}