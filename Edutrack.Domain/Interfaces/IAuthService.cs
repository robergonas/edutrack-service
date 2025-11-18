using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Interfaces
{
    public interface IAuthService
    {
        Task LogoutAsync(int userId, string refreshToken);
    }
}
