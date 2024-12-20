using Microsoft.Extensions.Caching.Memory;

namespace AvesdoSystemDesign.Infrastructure.Cache;

public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        memoryCache.Set(key, value, expiration);
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task RemoveAsync(string key)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
