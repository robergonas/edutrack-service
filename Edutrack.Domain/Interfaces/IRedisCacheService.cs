namespace EduTrack.Domain.Interfaces;

public interface IRedisCacheService
{
    Task InvalidateUserSessionsAsync(int userId);
    Task StoreSessionAsync(int userId, string refreshToken, bool rememberMe);
    Task<string?> GetRefreshTokenAsync(int userId);
    Task RemoveRefreshTokenAsync(int userId);
    Task<T?> GetCachedAsync<T>(string key);
    Task SetCachedAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveCachedAsync(string key);
    Task SetAsync(string key, string value, TimeSpan? expiration=null);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
}