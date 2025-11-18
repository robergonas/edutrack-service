using MediatR;
using EduTrack.Application.Authentication.Dtos;

public class LoginUserCommand : IRequest<AuthResponseDto>
{
    public string Username { get; set; } = string.Empty;
    public string Password{ get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}
