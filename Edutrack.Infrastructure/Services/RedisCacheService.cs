using EduTrack.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task InvalidateUserSessionsAsync(int userId)
    {
        await _cache.RemoveAsync($"session:{userId}");
    }

    public async Task StoreSessionAsync(int userId, string refreshToken, bool rememberMe)
    {
        var session = new { RefreshToken = refreshToken, RememberMe = rememberMe };
        var json = JsonSerializer.Serialize(session);
        await _cache.SetStringAsync($"session:{userId}", json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
        });
    }

    public async Task<string?> GetRefreshTokenAsync(int userId)
    {
        var json = await _cache.GetStringAsync($"session:{userId}");
        if (json == null) return null;
        var session = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        return session?["RefreshToken"];
    }

    public async Task RemoveRefreshTokenAsync(int userId)
    {
        await _cache.RemoveAsync($"session:{userId}");
    }

    public async Task<T?> GetCachedAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);
        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetCachedAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
        });
    }
    public async Task RemoveCachedAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiration;        

        await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
    }

    public async Task<string?> GetAsync(string key)
    {
        var data = await _cache.GetAsync(key);
        return data == null ? null : Encoding.UTF8.GetString(data);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}