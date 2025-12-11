using EduTrack.Application.Authentication.Commands;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Application.Authentication.Queries;
using MediatR;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/teachers")]
public class TeachersController : ControllerBase
{
    private readonly IMediator _mediator;
    public TeachersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
    {
        var result = await _mediator.Send(new CreateTeacherCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateTeacherDto dto)
    {
        var command = new UpdateTeacherCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        };

        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{teacherId:int}")]
    public async Task<IActionResult> Delete(int teacherId)
    {
        var command = new DeleteTeacherCommand
        {
            TeacherId = teacherId,
            CurrentUserName = User?.Identity?.Name ?? "system"
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { message = "Profesor eliminado correctamente." })
                      : BadRequest(new { message = "No se pudo eliminar el profesor." });
    }

    [HttpGet("{teacherId:int}")]
    public async Task<IActionResult> GetById(int teacherId)
    {
        var result = await _mediator.Send(new GetTeacherByIdQuery { TeacherId = teacherId });
        if (result == null) return NotFound(new { message = "Profesor no encontrado." });
        return Ok(result);
    }

    [HttpGet("List")]
    public async Task<IActionResult> List(
    [FromQuery] int? departmentId,
    [FromQuery] bool? status,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 15,
    [FromQuery] string employeeFullName = "")
    {
        var result = await _mediator.Send(new ListTeachersQuery
        {
            DepartmentId = departmentId,
            Status = status,
            EmployeeFullName = employeeFullName,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }


    [HttpGet("available-employees")]
    public async Task<IActionResult> GetAvailableEmployees()
    {
        var result = await _mediator.Send(new ListAvailableEmployeesQuery());
        return Ok(new
        {
            success = true,
            data = result
        });
    }

    [HttpPost("export_pdf")]
    public async Task<IActionResult> ExportPdf([FromBody] ExportTeachersPdfDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new ExportTeachersPdfQuery { Dto = dto }, ct);

        var fileName = $"reporte-profesores_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
        return File(result, "application/pdf", fileName);
    }
}