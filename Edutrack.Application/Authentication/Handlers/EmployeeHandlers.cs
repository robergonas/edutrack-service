using EduTrack.Application.Authentication.Commands;
using EduTrack.Domain.Entities;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeResponseDto>
{
    private readonly EduTrackDbContext _context;

    public CreateEmployeeHandler(EduTrackDbContext context) => _context = context;

    public async Task<EmployeeResponseDto> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        var depExists = await _context.Departments.AnyAsync(d => d.DepartmentId == request.Dto.DepartmentId, ct);
        if (!depExists) throw new Exception("Departamento inválido.");

        var posExists = await _context.Positions.AnyAsync(p => p.PositionId == request.Dto.PositionId, ct);
        if (!posExists) throw new Exception("Cargo/posición inválido.");

        var entity = new Employees
        {
            FullName = request.Dto.FullName,
            DepartmentId = request.Dto.DepartmentId,
            PositionId = request.Dto.PositionId,
            HireDate = request.Dto.HireDate,
            IsActive = request.Dto.IsActive,
            CreatedAt = DateTime.Now,
            CreatedBy = request.CurrentUserName
        };

        _context.Employees.Add(entity);
        await _context.SaveChangesAsync(ct);

        var dep = await _context.Departments.FirstAsync(d => d.DepartmentId == entity.DepartmentId, ct);
        var pos = await _context.Positions.FirstAsync(p => p.PositionId == entity.PositionId, ct);

        return new EmployeeResponseDto
        {
            EmployeeId = entity.EmployeeId,
            FullName = entity.FullName,
            DepartmentId = entity.DepartmentId,
            DepartmentName = dep.DepartmentName,
            PositionId = entity.PositionId,
            PositionName = pos.PositionName,
            HireDate = entity.HireDate,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
    }
}

public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeResponseDto>
{
    private readonly EduTrackDbContext _context;

    public UpdateEmployeeHandler(EduTrackDbContext context) => _context = context;

    public async Task<EmployeeResponseDto> Handle(UpdateEmployeeCommand request, CancellationToken ct)
    {
        var e = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == request.Dto.EmployeeId, ct);
        if (e == null) throw new Exception("Empleado no encontrado.");

        if (request.Dto.FullName != null) e.FullName = request.Dto.FullName;

        if (request.Dto.DepartmentId.HasValue)
        {
            var depExists = await _context.Departments.AnyAsync(d => d.DepartmentId == request.Dto.DepartmentId.Value, ct);
            if (!depExists) throw new Exception("Departamento inválido.");
            e.DepartmentId = request.Dto.DepartmentId.Value;
        }

        if (request.Dto.PositionId.HasValue)
        {
            var posExists = await _context.Positions.AnyAsync(p => p.PositionId == request.Dto.PositionId.Value, ct);
            if (!posExists) throw new Exception("Cargo/posición inválido.");
            e.PositionId = request.Dto.PositionId.Value;
        }

        if (request.Dto.HireDate.HasValue) e.HireDate = request.Dto.HireDate.Value;
        if (request.Dto.IsActive.HasValue) e.IsActive = request.Dto.IsActive.Value;

        e.ModifiedAt = DateTime.Now;
        e.ModifiedBy = request.CurrentUserName;

        _context.Employees.Update(e);
        await _context.SaveChangesAsync(ct);

        var dep = await _context.Departments.FirstAsync(d => d.DepartmentId == e.DepartmentId, ct);
        var pos = await _context.Positions.FirstAsync(p => p.PositionId == e.PositionId, ct);

        return new EmployeeResponseDto
        {
            EmployeeId = e.EmployeeId,
            FullName = e.FullName,
            DepartmentId = e.DepartmentId,
            DepartmentName = dep.DepartmentName,
            PositionId = e.PositionId,
            PositionName = pos.PositionName,
            HireDate = e.HireDate,
            IsActive = e.IsActive,
            CreatedAt = e.CreatedAt
        };
    }
}

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly EduTrackDbContext _context;

    public DeleteEmployeeHandler(EduTrackDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken ct)
    {
        var e = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == request.EmployeeId, ct);
        if (e == null) throw new Exception("Empleado no encontrado.");

        e.IsActive = false;
        e.ModifiedAt = DateTime.Now;
        e.ModifiedBy = request.CurrentUserName;

        _context.Employees.Update(e);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}

public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponseDto?>
{
    private readonly EduTrackDbContext _context;

    public GetEmployeeByIdHandler(EduTrackDbContext context) => _context = context;

    public async Task<EmployeeResponseDto?> Handle(GetEmployeeByIdQuery request, CancellationToken ct)
    {
        var e = await _context.Employees
            .Include(x => x.Department)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.EmployeeId == request.EmployeeId, ct);
        if (e == null) return null;

        return new EmployeeResponseDto
        {
            EmployeeId = e.EmployeeId,
            FullName = e.FullName,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department.DepartmentName,
            PositionId = e.PositionId,
            PositionName = e.Position.PositionName,
            HireDate = e.HireDate,
            IsActive = e.IsActive,
            CreatedAt = e.CreatedAt
        };
    }
}

public class ListEmployeesHandler : IRequestHandler<ListEmployeesQuery, IReadOnlyList<EmployeeResponseDto>>
{
    private readonly EduTrackDbContext _context;

    public ListEmployeesHandler(EduTrackDbContext context) => _context = context;

    public async Task<IReadOnlyList<EmployeeResponseDto>> Handle(ListEmployeesQuery request, CancellationToken ct)
    {
        var q = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .AsQueryable();

        if (request.IsActive.HasValue) q = q.Where(e => e.IsActive == request.IsActive.Value);
        if (request.DepartmentId.HasValue) q = q.Where(e => e.DepartmentId == request.DepartmentId.Value);
        if (request.PositionId.HasValue) q = q.Where(e => e.PositionId == request.PositionId.Value);

        return await q
            .Select(e => new EmployeeResponseDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.DepartmentName,
                PositionId = e.PositionId,
                PositionName = e.Position.PositionName,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .OrderBy(e => e.FullName)
            .ToListAsync(ct);
    }
}

public class ListEmployeeSelectHandler : IRequestHandler<ListEmployeeSelectQuery, IReadOnlyList<EmployeeSelectDto>>
{
    private readonly EduTrackDbContext _context;

    public ListEmployeeSelectHandler(EduTrackDbContext context) => _context = context;

    public async Task<IReadOnlyList<EmployeeSelectDto>> Handle(ListEmployeeSelectQuery request, CancellationToken ct)
    {
        var q = _context.Employees.AsQueryable();
        if (request.IsActive.HasValue) q = q.Where(e => e.IsActive == request.IsActive.Value);
        if (request.DepartmentId.HasValue) q = q.Where(e => e.DepartmentId == request.DepartmentId.Value);

        return await q
            .Select(e => new EmployeeSelectDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                DepartmentId = e.DepartmentId,
                PositionId = e.PositionId
            })
            .OrderBy(e => e.FullName)
            .ToListAsync(ct);
    }
}