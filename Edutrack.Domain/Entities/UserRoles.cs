using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class UserRoles
    {
        public int UserRoleId { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public int RoleId { get; set; } = 0;        
    }
}
