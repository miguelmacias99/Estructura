using Cititrans.Auth.Entidad.Modelo.Identity;

namespace Cititrans.Auth.RepositorioEfCore.Extensiones
{
    public static class TwoFactorTypeExtensions
    {
        public static TwoFactorType ObtenerDesdeNumero(int number)
        {
            return Enum.TryParse(typeof(TwoFactorType), number.ToString(), out var result)
                ? (TwoFactorType)result
                : throw new ArgumentOutOfRangeException(nameof(number), $"El valor {number} no es válido para TwoFactorType.");
        }
    }
}