using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace AvesdoSystemDesign.Infrastructure.Cache;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await distributedCache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await distributedCache.GetStringAsync(key);
        return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveAsync(string key)
    {
        await distributedCache.RemoveAsync(key);
    }
}
