using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Application.Authentication.Dtos
{
    public class CreateTeacherDto
    {
        public int EmployeeId { get; set; }
        public string? Specialty { get; set; }
        public string? Degree { get; set; }
        public DateTime HireDate { get; set; }
        public bool Status { get; set; } = true;
    }
    public class UpdateTeacherDto
    {
        public int TeacherId { get; set; }
        public string? Specialty { get; set; }
        public string? Degree { get; set; }
        public DateTime HireDate { get; set; }
        public bool Status { get; set; }
    }
    public class TeacherResponseDto
    {
        public int TeacherId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public string? Degree { get; set; }
        public DateTime HireDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; } = string.Empty;
        public int? PositionId { get; set; }
        public string? PositionName { get; set; } = string.Empty;

    }
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        public PagedResult(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
    public class ExportTeachersFilter
    {
        public int? DepartmentId { get; set; }
        public string? EmployeeFullName { get; set; }
        public bool? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
    public class ExportTeacherRow
    {
        public string EmployeeFullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; }
        public string? Specialty { get; set; }
        public string? Degree { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public bool Status { get; set; }
    }
    public class ReportStatistics
    {
        public int TotalTeachers { get; set; }
        public int ActiveTeachers { get; set; }
        public int InactiveTeachers { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
    public class ExportTeachersPdfDto
    {
        public int? DepartmentId { get; set; }
        public string? EmployeeFullName { get; set; }
        public bool? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

}
