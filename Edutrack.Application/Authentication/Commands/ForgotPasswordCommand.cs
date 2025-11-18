using MediatR;

namespace EduTrack.Application.Authentication.Commands
{
    public record ForgotPasswordCommand(string Username, string Email) : IRequest<Unit>;
}
