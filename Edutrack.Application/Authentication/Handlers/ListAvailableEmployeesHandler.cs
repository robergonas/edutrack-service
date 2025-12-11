using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class ListAvailableEmployeesQuery : IRequest<IReadOnlyList<EmployeeSelectDto>> { }

public class ListAvailableEmployeesHandler
    : IRequestHandler<ListAvailableEmployeesQuery, IReadOnlyList<EmployeeSelectDto>>
{
    private readonly EduTrackDbContext _context;
    public ListAvailableEmployeesHandler(EduTrackDbContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<EmployeeSelectDto>> Handle(ListAvailableEmployeesQuery request, CancellationToken ct)
    {
        var availableEmployees = await _context.Employees
            .Where(e => !_context.Teachers.Any(t => t.EmployeeId == e.EmployeeId))
            .Select(e => new EmployeeSelectDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                DepartmentId = e.DepartmentId,
                PositionId = e.PositionId,
                IsActive = e.IsActive,
                DepartmentName = e.Department.DepartmentName,
                PositionName = e.Position.PositionName 
            })
            .OrderBy(e => e.FullName)
            .ToListAsync(ct);

        return availableEmployees;
    }
}