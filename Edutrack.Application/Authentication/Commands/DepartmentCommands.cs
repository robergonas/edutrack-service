using EduTrack.Application.Authentication.Dtos;
using MediatR;

namespace EduTrack.Application.Authentication.Commands
{
    public class CreateDepartmentCommand : IRequest<DepartmentResponseDto>
    {
        public CreateDepartmentDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class UpdateDepartmentCommand : IRequest<DepartmentResponseDto>
    {
        public UpdateDepartmentDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public int DepartmentId { get; set; }
        public string CurrentUserName { get; set; } = "system";
    }
    public class GetDepartmentByIdQuery : IRequest<DepartmentResponseDto?>
    {
        public int DepartmentId { get; set; }
    }

    public class ListDepartmentsQuery : IRequest<IReadOnlyList<DepartmentResponseDto>>
    {
        public bool? IsActive { get; set; }
    }
    public class ListDepartmentSelectQuery : IRequest<IReadOnlyList<DepartmentSelectDto>>
    {
        public bool? IsActive { get; set; }
    }

}