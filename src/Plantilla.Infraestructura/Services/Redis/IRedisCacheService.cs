using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Distributed;

namespace Plantilla.Infraestructura.Services.Redis
{
    public interface IRedisCacheService
    {
        Task EliminarClavesPorPatronAsync(string patron, [CallerMemberName] string metodoInvoca = "");

        Task<T?> ObtenerCache<T>(string cacheKey, [CallerMemberName] string metodoInvoca = "");

        Task GuardarCache(string cacheKey, object objeto, DistributedCacheEntryOptions? options = null, [CallerMemberName] string metodoInvoca = "");
    }
}