using EduTrack.Application.Authentication.Dtos;
using MediatR;

namespace EduTrack.Application.Authentication.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }=string.Empty;
        public string NewPassword { get; set; }=string.Empty;
    };
}
