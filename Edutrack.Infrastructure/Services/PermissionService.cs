using EduTrack.Domain.Interfaces;
using EduTrack.Domain.Models.Views;
using EduTrack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Infrastructure.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly EduTrackDbContext _context;
        public PermissionService(EduTrackDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserEffectivePermission>> GetPermissionsByUserIdAsync(int userId)
        {
            return await _context.UserEffectivePermissions
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

    }
}
