using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class Employees
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int DepartmentID { get; set; } = 0;
        public int PositionID { get; set; } = 0;        
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;        
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }        
    }
}
