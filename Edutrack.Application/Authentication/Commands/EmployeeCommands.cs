using MediatR;

namespace EduTrack.Application.Authentication.Commands {
    public class CreateEmployeeCommand : IRequest<EmployeeResponseDto>
    {
        public CreateEmployeeDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class UpdateEmployeeCommand : IRequest<EmployeeResponseDto>
    {
        public UpdateEmployeeDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public string CurrentUserName { get; set; } = "system";
    }
    public class GetEmployeeByIdQuery : IRequest<EmployeeResponseDto?>
    {
        public int EmployeeId { get; set; }
    }
    public class ListEmployeesQuery : IRequest<IReadOnlyList<EmployeeResponseDto>>
    {
        public bool? IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
    }

    public class ListEmployeeSelectQuery : IRequest<IReadOnlyList<EmployeeSelectDto>>
    {
        public bool? IsActive { get; set; }
        public int? DepartmentId { get; set; }
    }
}