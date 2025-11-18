using EduTrack.Domain.Models.Views;

namespace EduTrack.Domain.Interfaces
{
    public interface IPermissionService
    {
        Task<List<UserEffectivePermission>> GetPermissionsByUserIdAsync(int userId);

    }
}
