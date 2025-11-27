using EduTrack.Application.Authentication.Commands;
using EduTrack.Domain.Entities;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Application.Authentication.Handlers
{
    #region Create
    public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, PositionResponseDto>
    {
        private readonly EduTrackDbContext _context;
        public CreatePositionHandler(EduTrackDbContext context) => _context = context;

        public async Task<PositionResponseDto> Handle(CreatePositionCommand request, CancellationToken ct)
        {
            var exists = await _context.Positions
                .AnyAsync(p => p.PositionName == request.Dto.PositionName, ct);
            if (exists) throw new Exception("El nombre de posición ya existe.");

            var entity = new Position
            {
                PositionName = request.Dto.PositionName,
                Description = request.Dto.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CurrentUserName
            };

            _context.Positions.Add(entity);
            await _context.SaveChangesAsync(ct);

            return new PositionResponseDto
            {
                PositionId = entity.PositionId,
                PositionName = entity.PositionName,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt
            };
        }
    }
    #endregion

    #region Update
    public class UpdatePositionHandler : IRequestHandler<UpdatePositionCommand, PositionResponseDto>
    {
        private readonly EduTrackDbContext _context;

        public UpdatePositionHandler(EduTrackDbContext context) => _context = context;

        public async Task<PositionResponseDto> Handle(UpdatePositionCommand request, CancellationToken ct)
        {
            var entity = await _context.Positions.FirstOrDefaultAsync(p => p.PositionId == request.Dto.PositionId, ct);
            if (entity == null) throw new Exception("Posición no encontrada.");

            if (request.Dto.PositionName != null)
            {
                var exists = await _context.Positions
                    .AnyAsync(p => p.PositionName == request.Dto.PositionName && p.PositionId != entity.PositionId, ct);
                if (exists) throw new Exception("Otro registro ya usa ese nombre.");
                entity.PositionName = request.Dto.PositionName;
            }

            if (request.Dto.Description != null) entity.Description = request.Dto.Description;

            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = request.CurrentUserName;

            _context.Positions.Update(entity);
            await _context.SaveChangesAsync(ct);

            return new PositionResponseDto
            {
                PositionId = entity.PositionId,
                PositionName = entity.PositionName,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt
            };
        }
    }
    #endregion

    #region Delete
    public class DeletePositionHandler : IRequestHandler<DeletePositionCommand, bool>
    {
        private readonly EduTrackDbContext _context;

        public DeletePositionHandler(EduTrackDbContext context) => _context = context;

        public async Task<bool> Handle(DeletePositionCommand request, CancellationToken ct)
        {
            var entity = await _context.Positions.FirstOrDefaultAsync(p => p.PositionId == request.PositionId, ct);
            if (entity == null) throw new Exception("Posición no encontrada.");

            _context.Positions.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
    #endregion

    #region GetById
    public class GetPositionByIdHandler : IRequestHandler<GetPositionByIdQuery, PositionResponseDto?>
    {
        private readonly EduTrackDbContext _context;

        public GetPositionByIdHandler(EduTrackDbContext context) => _context = context;

        public async Task<PositionResponseDto?> Handle(GetPositionByIdQuery request, CancellationToken ct)
        {
            var p = await _context.Positions.FirstOrDefaultAsync(x => x.PositionId == request.PositionId, ct);
            if (p == null) return null;

            return new PositionResponseDto
            {
                PositionId = p.PositionId,
                PositionName = p.PositionName,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            };
        }
    }
    #endregion

    #region List
    public class ListPositionsHandler : IRequestHandler<ListPositionsQuery, IReadOnlyList<PositionResponseDto>>
    {
        private readonly EduTrackDbContext _context;

        public ListPositionsHandler(EduTrackDbContext context) => _context = context;

        public async Task<IReadOnlyList<PositionResponseDto>> Handle(ListPositionsQuery request, CancellationToken ct)
        {
            return await _context.Positions
                .Select(p => new PositionResponseDto
                {
                    PositionId = p.PositionId,
                    PositionName = p.PositionName,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                })
                .OrderBy(p => p.PositionName)
                .ToListAsync(ct);
        }
    }
    #endregion

    #region Select (para combos)
    public class ListPositionSelectHandler : IRequestHandler<ListPositionSelectQuery, IReadOnlyList<PositionSelectDto>>
    {
        private readonly EduTrackDbContext _context;

        public ListPositionSelectHandler(EduTrackDbContext context) => _context = context;

        public async Task<IReadOnlyList<PositionSelectDto>> Handle(ListPositionSelectQuery request, CancellationToken ct)
        {
            return await _context.Positions
                .Select(p => new PositionSelectDto
                {
                    PositionId = p.PositionId,
                    PositionName = p.PositionName
                })
                .OrderBy(p => p.PositionName)
                .ToListAsync(ct);
        }
    }
    #endregion
}
