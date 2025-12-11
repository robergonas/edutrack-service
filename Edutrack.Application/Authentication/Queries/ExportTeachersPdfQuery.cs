using EduTrack.Application.Authentication.Dtos;
using MediatR;

namespace EduTrack.Application.Authentication.Queries
{
    public class ExportTeachersPdfQuery : IRequest<byte[]>
    {
        public ExportTeachersPdfDto Dto { get; set; } = new();

    }
}
