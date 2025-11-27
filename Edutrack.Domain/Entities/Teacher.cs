using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public int EmployeeId { get; set; }
        public string? Specialty { get; set; }
        public string? Degree { get; set; }
        public DateTime HireDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public Employees Employee { get; set; } = null!;
    }
}