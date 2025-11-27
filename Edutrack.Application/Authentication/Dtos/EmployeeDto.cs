public class CreateEmployeeDto
{
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;
}
public class UpdateEmployeeDto
{
    public int EmployeeId { get; set; }
    public string? FullName { get; set; }
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public DateTime? HireDate { get; set; }
    public bool? IsActive { get; set; }
}
public class EmployeeResponseDto
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class EmployeeSelectDto
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int PositionId { get; set; }
    public string PositionName { get; set; }
    public bool IsActive { get; set; }
}