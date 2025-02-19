namespace Plantilla.Infraestructura.Constantes
{
    public static class Servidor
    {
        public static readonly string ExitoComun = "00000";
        public static readonly string ErrorComun = "00099";
        public static readonly string ErrorBaseDatos = "00088";
        public static readonly string SesionInvalida = "00077";
        public static readonly string Excepcion = "99999";

        public static readonly string[] CodigosNoLog =
        [
            ExitoComun,
            Excepcion
        ];
    }
}