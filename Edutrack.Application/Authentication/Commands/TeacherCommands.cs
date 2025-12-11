using EduTrack.Application.Authentication.Dtos;
using MediatR;

namespace EduTrack.Application.Authentication.Commands
{
    public class CreateTeacherCommand : IRequest<TeacherResponseDto>
    {
        public CreateTeacherDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class UpdateTeacherCommand : IRequest<TeacherResponseDto>
    {
        public UpdateTeacherDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class DeleteTeacherCommand : IRequest<bool>
    {
        public int TeacherId { get; set; }
        public string CurrentUserName { get; set; } = "system";
    }
    public class GetTeacherByIdQuery : IRequest<TeacherResponseDto?>
    {
        public int TeacherId { get; set; }
    }
    public class ListTeachersQuery : IRequest<PagedResult<TeacherResponseDto>>
    {
        public int? DepartmentId { get; set; }
        public bool? Status { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;

        public string EmployeeFullName { get; set; }
    }
}
