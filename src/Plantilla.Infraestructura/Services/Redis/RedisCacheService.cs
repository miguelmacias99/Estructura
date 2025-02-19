using System.Runtime.CompilerServices;
using System.Text;
using Plantilla.Infraestructura.Services.Encriptacion;
using Plantilla.Infraestructura.Utilidades.Logger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Plantilla.Infraestructura.Services.Redis
{
    internal class RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redisConnection,
        IConfiguration configuration, IEncriptarTextoService encriptarTexto) : IRedisCacheService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly IConnectionMultiplexer _redisConnection = redisConnection;
        private readonly IEncriptarTextoService _encriptarTexto = encriptarTexto;

        private readonly DistributedCacheEntryOptions _options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToDouble(configuration["Redis:AbsoluteExpirationMinutes"])))
                .SetSlidingExpiration(TimeSpan.FromMinutes(Convert.ToDouble(configuration["Redis:SlidingExpirationMinutes"])));

        public async Task<T?> ObtenerCache<T>(string cacheKey, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                var objetoCache = await _cache.GetAsync(cacheKey);
                if (objetoCache is null) return default;

                var textoEncriptado = Encoding.UTF8.GetString(objetoCache);
                var objetoSerializado = _encriptarTexto.Desencriptar(textoEncriptado);
                return JsonConvert.DeserializeObject<T>(objetoSerializado);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, metodoInvoca: metodoInvoca);
                throw;
            }
        }

        public async Task GuardarCache(string cacheKey, object objeto,
            DistributedCacheEntryOptions? options = null, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                var objetoSerializado = JsonConvert.SerializeObject(objeto)!;
                var textoEncriptado = _encriptarTexto.Encriptar(objetoSerializado);
                var objetoBytes = Encoding.UTF8.GetBytes(textoEncriptado);

                options ??= _options;

                await _cache.SetAsync(cacheKey, objetoBytes, options);
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, metodoInvoca: metodoInvoca);
                throw;
            }
        }

        public async Task EliminarClavesPorPatronAsync(string patron, [CallerMemberName] string metodoInvoca = "")
        {
            try
            {
                var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
                var keys = server.Keys(pattern: patron + "*").ToArray(); // Busca las claves que coinciden con el patr�n

                if (keys.Length != 0)
                {
                    var db = _redisConnection.GetDatabase();
                    await db.KeyDeleteAsync(keys); // Elimina las claves encontradas
                    LogUtils.LogInformation($"Se eliminaron {keys.Length} claves que coinciden con el patr�n '{patron}*'.", metodoInvoca: metodoInvoca);
                }
                else
                {
                    LogUtils.LogInformation($"No se encontraron claves que coincidan con el patr�n '{patron}*'.", metodoInvoca: metodoInvoca);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, metodoInvoca: metodoInvoca);
                throw;
            }
        }
    }
}