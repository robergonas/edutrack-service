using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Models.Views
{
    public class UserEffectivePermission
    {
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public string RoleName { get; set; } = default!;
        public string Module { get; set; } = default!;
        public string AccessType { get; set; } = default!;
        public int AccessTypeId { get; set; } = default!;
        public string? PermissionDescription { get; set; }
    }

}
