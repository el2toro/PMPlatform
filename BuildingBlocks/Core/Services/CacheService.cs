using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Core.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken, TimeSpan? absoluteExpireTime = null);
    Task DeleteAsync<T>(string key, CancellationToken cancellationToken);
}

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    public async Task DeleteAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (value is null) return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default, TimeSpan? absoluteExpireTime = null)
    {
        var serializedData = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30)
        };

        await _distributedCache.SetStringAsync(key, serializedData, options, cancellationToken);
    }
}
