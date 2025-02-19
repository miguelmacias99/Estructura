using System.Security.Cryptography;

namespace Plantilla.Infraestructura.Utilidades.Encriptacion
{
    public static class ContraseniasUtil
    {
        public static (string hash, string salt) EncriptarContrasenia(string contrasenia)
        {
            // Generar un salt aleatorio
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Usar PBKDF2 para generar el hash con salt
            using var pbkdf2 = new Rfc2898DeriveBytes(contrasenia, saltBytes, 100_000, HashAlgorithmName.SHA256); // 100,000 iteraciones
            byte[] hashBytes = pbkdf2.GetBytes(32); // 256-bit hash
            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return (hash, salt);
        }

        public static bool VerificarContrasenia(string contrasenia, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var pbkdf2 = new Rfc2898DeriveBytes(contrasenia, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            string computedHash = Convert.ToBase64String(hashBytes);

            // Compara el hash almacenado con el hash calculado
            return computedHash == storedHash;
        }
    }
}