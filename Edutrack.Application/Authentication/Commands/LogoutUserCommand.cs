using MediatR;

namespace EduTrack.Application.Authentication.Commands
{
    public record LogoutUserCommand(int UserId, string RefreshToken) : IRequest<Unit>;
}
