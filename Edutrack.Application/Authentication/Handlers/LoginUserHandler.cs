using MediatR;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EduTrack.Domain.Interfaces;
using EduTrack.Domain.Entities;
using EduTrack.Domain.Models.Views;
using EduTrack.Application.Authentication.Commands;
public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly EduTrackDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<LoginUserHandler> _logger;

    public LoginUserHandler(EduTrackDbContext context, IJwtService jwtService, IRedisCacheService cache, ILogger<LoginUserHandler> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users            
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

            var employees = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == user.EmployeeId, cancellationToken);

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _cache.InvalidateUserSessionsAsync(user.UserId);
            await _cache.StoreSessionAsync(user.UserId, refreshToken, request.RememberMe);

            user.LastLogin = DateTime.UtcNow;
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = request.Username;

            user.UserRoles =await _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)                
                .ToListAsync(cancellationToken);
            
            var permissions= await _context.UserEffectivePermissions
                .Where(uep => uep.UserId == user.UserId)
                .ToListAsync(cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                UserName = user.Username,
                UserRoles = user.UserRoles,
                Employees = employees,
                UserID= user.UserId,
                Permissions= (ICollection<UserEffectivePermission>)permissions
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}