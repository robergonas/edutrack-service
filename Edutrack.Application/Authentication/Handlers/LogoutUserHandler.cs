using EduTrack.Application.Authentication.Commands;
using EduTrack.Domain.Interfaces;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly EduTrackDbContext _context;
    private readonly IRedisCacheService _redis;
    private readonly ILogger<LogoutUserHandler> _logger;

    public LogoutUserHandler(EduTrackDbContext context, IRedisCacheService redis, ILogger<LogoutUserHandler> logger)
    {
        _context = context;
        _redis = redis;
        _logger = logger;
    }

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        // Revocar sesión activa en base de datos
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.UserId == request.UserId && s.RefreshToken == request.RefreshToken && s.IsActive, cancellationToken);

        if (session != null)
        {
            session.IsActive = false;
            session.RevokedAt = DateTime.UtcNow;
            session.ModifiedAt = DateTime.UtcNow;
            session.ModifiedBy = "logout";
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Eliminar sesión en Redis (si aplica)
        var redisKey = $"session:{request.UserId}";
        await _redis.RemoveAsync(redisKey);

        // Registrar auditoría
        _logger.LogInformation("Usuario {UserId} cerró sesión correctamente.", request.UserId);

        return Unit.Value;
    }
}