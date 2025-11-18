using EduTrack.Domain.Entities;
using EduTrack.Domain.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Application.Authentication.Dtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserName { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public int UserID { get; set; }
        public Employees Employees { get; set; }
        public ICollection<UserEffectivePermission> Permissions { get; set; }
    }
}
