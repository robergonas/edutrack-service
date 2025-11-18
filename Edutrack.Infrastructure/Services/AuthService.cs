using EduTrack.Domain.Interfaces;
using EduTrack.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduTrack.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly EduTrackDbContext _context;
        private readonly IRedisCacheService _redis;
        private readonly ILogger<AuthService> _logger;

        public AuthService(EduTrackDbContext context, IRedisCacheService redis, ILogger<AuthService> logger)
        {
            _context = context;
            _redis = redis;
            _logger = logger;
        }

        public async Task LogoutAsync(int userId, string refreshToken)
        {
            // 1. Revocar sesión en base de datos (si aplica)
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.RefreshToken == refreshToken && s.IsActive);

            if (session != null)
            {
                session.IsActive = false;
                session.RevokedAt = DateTime.UtcNow;
                session.ModifiedAt = DateTime.UtcNow;
                session.ModifiedBy = "logout";
                await _context.SaveChangesAsync();
            }

            // 2. Eliminar token de Redis (si lo usas para control de sesión)
            var redisKey = $"session:{userId}";
            await _redis.RemoveAsync(redisKey);

            // 3. Registrar auditoría (opcional)
            _logger.LogInformation("Usuario {UserId} cerró sesión correctamente.", userId);
        }

    }
}
