using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Plantilla.Infraestructura.Services.Encriptacion
{
    internal class EncriptarTextoService(IConfiguration configuration) : IEncriptarTextoService
    {
        private readonly string Key = configuration["Seguridad:Key"]!;
        private readonly string Iv = configuration["Seguridad:Inicializador"]!;

        public string Desencriptar(string texto)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Convert.FromBase64String(Key);
            aesAlg.IV = Convert.FromBase64String(Iv);

            var bytesDesencriptar = Convert.FromBase64String(texto);
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var ms = new MemoryStream(bytesDesencriptar);
            using CryptoStream cryptoStream = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new(cryptoStream);

            // Devolver directamente el texto desencriptado
            var base64Data = reader.ReadToEnd();
            var bytesData = Convert.FromBase64String(base64Data);
            var textoRetorno = Encoding.UTF8.GetString(bytesData);
            return textoRetorno;
        }

        public string Encriptar(string texto)
        {
            var bytesData = Encoding.UTF8.GetBytes(texto);
            var base64Data = Convert.ToBase64String(bytesData);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Convert.FromBase64String(Key);
            aesAlg.IV = Convert.FromBase64String(Iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var ms = new MemoryStream();
            using CryptoStream cryptoStream = new(ms, encryptor, CryptoStreamMode.Write);
            using StreamWriter writer = new(cryptoStream);

            writer.Write(base64Data);
            writer.Flush(); // Asegúrate de que todos los datos se escriban en el stream
            cryptoStream.FlushFinalBlock();

            var textoEncriptado = Convert.ToBase64String(ms.ToArray());
            return textoEncriptado;
        }
    }
}