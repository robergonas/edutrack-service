
namespace EduTrack.Domain.Entities
{
    public class Employees
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int DepartmentId { get; set; } = 0;
        public int PositionId { get; set; } = 0;        
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;        
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public Department Department { get; set; } = null;
        public Position Position { get; set; } = null;
    }
}
