using EduTrack.Application.Authentication.Commands;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Domain.Entities;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, DepartmentResponseDto>
{
    private readonly EduTrackDbContext _context;

    public CreateDepartmentHandler(EduTrackDbContext context) => _context = context;

    public async Task<DepartmentResponseDto> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var exists = await _context.Departments
            .AnyAsync(d => d.DepartmentName == request.Dto.DepartmentName, ct);
        if (exists) throw new Exception("El nombre de departamento ya existe.");

        var entity = new Department
        {
            DepartmentName = request.Dto.DepartmentName,
            Description = request.Dto.Description,            
            CreatedAt = DateTime.Now, // default getdate() en BD, se mantiene coherencia
            CreatedBy = request.CurrentUserName
        };

        _context.Departments.Add(entity);
        await _context.SaveChangesAsync(ct);

        return new DepartmentResponseDto
        {
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.DepartmentName,
            Description = entity.Description,            
            CreatedAt = entity.CreatedAt
        };
    }
}
public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentResponseDto>
{
    private readonly EduTrackDbContext _context;

    public UpdateDepartmentHandler(EduTrackDbContext context) => _context = context;

    public async Task<DepartmentResponseDto> Handle(UpdateDepartmentCommand request, CancellationToken ct)
    {
        var entity = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == request.Dto.DepartmentId, ct);
        if (entity == null) throw new Exception("Departamento no encontrado.");

        if (request.Dto.DepartmentName != null)
        {
            var exists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == request.Dto.DepartmentName && d.DepartmentId != entity.DepartmentId, ct);
            if (exists) throw new Exception("Otro departamento ya usa ese nombre.");
            entity.DepartmentName = request.Dto.DepartmentName;
        }

        if (request.Dto.Description != null) entity.Description = request.Dto.Description;
        
        entity.ModifiedAt = DateTime.Now;
        entity.ModifiedBy = request.CurrentUserName;

        _context.Departments.Update(entity);
        await _context.SaveChangesAsync(ct);

        return new DepartmentResponseDto
        {
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.DepartmentName,
            Description = entity.Description,            
            CreatedAt = entity.CreatedAt
        };
    }
}
public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly EduTrackDbContext _context;

    public DeleteDepartmentHandler(EduTrackDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken ct)
    {
        var entity = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == request.DepartmentId, ct);
        if (entity == null) throw new Exception("Departamento no encontrado.");

        // Borrado lógico        
        entity.ModifiedAt = DateTime.Now;
        entity.ModifiedBy = request.CurrentUserName;

        _context.Departments.Update(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentResponseDto?>
{
    private readonly EduTrackDbContext _context;

    public GetDepartmentByIdHandler(EduTrackDbContext context) => _context = context;

    public async Task<DepartmentResponseDto?> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
    {
        var d = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == request.DepartmentId, ct);
        if (d == null) return null;

        return new DepartmentResponseDto
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName,
            Description = d.Description,            
            CreatedAt = d.CreatedAt
        };
    }
}
public class ListDepartmentsHandler : IRequestHandler<ListDepartmentsQuery, IReadOnlyList<DepartmentResponseDto>>
{
    private readonly EduTrackDbContext _context;

    public ListDepartmentsHandler(EduTrackDbContext context) => _context = context;

    public async Task<IReadOnlyList<DepartmentResponseDto>> Handle(ListDepartmentsQuery request, CancellationToken ct)
    {
        var q = _context.Departments.AsQueryable();        

        return await q
            .Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Description = d.Description,                
                CreatedAt = d.CreatedAt
            })
            .OrderBy(d => d.DepartmentName)
            .ToListAsync(ct);
    }
}
public class ListDepartmentSelectHandler : IRequestHandler<ListDepartmentSelectQuery, IReadOnlyList<DepartmentSelectDto>>
{
    private readonly EduTrackDbContext _context;
    public ListDepartmentSelectHandler(EduTrackDbContext context) => _context = context;

    public async Task<IReadOnlyList<DepartmentSelectDto>> Handle(ListDepartmentSelectQuery request, CancellationToken ct)
    {
        var q = _context.Departments.AsQueryable();        

        return await q
            .Select(d => new DepartmentSelectDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .OrderBy(d => d.DepartmentName)
            .ToListAsync(ct);
    }
}