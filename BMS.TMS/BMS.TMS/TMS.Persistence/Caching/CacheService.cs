using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TMS.Persistence.Caching;

public class CacheService(IDistributedCache _cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await _cache.GetAsync(key, cancellationToken);

        return bytes is null ? default : Derserialize<T>(bytes);
    }

    private T Derserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        return _cache.SetAsync(key, bytes, GetOptions(expiration), cancellationToken);
    }

    private byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }

    private DistributedCacheEntryOptions GetOptions(TimeSpan? expiration)
    {
        var opts = new DistributedCacheEntryOptions();

        if (expiration is not null)
        {
            opts.AbsoluteExpirationRelativeToNow = expiration;
        }
        else
        {
            opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
        }

        return opts;
    }
}