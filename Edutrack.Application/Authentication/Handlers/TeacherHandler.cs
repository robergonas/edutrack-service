using EduTrack.Application.Authentication.Commands;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Domain.Entities;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Application.Authentication.Handlers

{
    public class CreateTeacherHandler : IRequestHandler<ListTeachersQuery, PagedResult<TeacherResponseDto>>
    {
        private readonly EduTrackDbContext _context;

        public CreateTeacherHandler(EduTrackDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<TeacherResponseDto>> Handle(ListTeachersQuery request, CancellationToken ct)
        {
            var query = _context.Teachers
            .Include(t => t.Employee)
            .AsQueryable();

            // Filtrar por estado
            if (request.Status.HasValue)
                query = query.Where(t => t.Status == request.Status.Value);

            // Filtrar por departamento (si departmentId != 0)
            if (request.DepartmentId.HasValue && request.DepartmentId.Value != 0)
                query = query.Where(t => t.Employee.DepartmentID == request.DepartmentId.Value);

            //filtrar por nombre
            if (!string.IsNullOrWhiteSpace(request.EmployeeFullName))
                query=query.Where(t => t.Employee.FullName.Contains(request.EmployeeFullName));

            // Total antes de paginar
            var totalCount = await query.CountAsync(ct);

            // Aplicar paginación dinámica
            var items = await query
            .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
            .Include(t => t.Employee)
                .ThenInclude(e => e.Position)
            .OrderBy(t => t.Employee.FullName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                EmployeeId = t.EmployeeId,
                EmployeeFullName = t.Employee.FullName,
                Specialty = t.Specialty,
                Degree = t.Degree,
                HireDate = t.HireDate,
                Status = t.Status,
                CreatedAt = t.CreatedAt,

                DepartmentId = t.Employee.DepartmentID,
                DepartmentName = t.Employee.Department.DepartmentName,

                PositionId = t.Employee.PositionID,
                PositionName = t.Employee.Position.PositionName
            })
            .ToListAsync(ct);


            return new PagedResult<TeacherResponseDto>
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Items = items
            };
        }
    }
}
