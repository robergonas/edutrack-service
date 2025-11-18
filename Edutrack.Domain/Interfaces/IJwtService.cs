using EduTrack.Domain.Entities;
using System.Security.Claims;

namespace EduTrack.Domain.Interfaces;
public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateAccessToken(string token);
}