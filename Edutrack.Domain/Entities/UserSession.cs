using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class UserSession
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public string? DeviceInfo { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public User? User { get; set; }

    }
}
