using EduTrack.Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeDto dto)
    {
        var result = await _mediator.Send(new UpdateEmployeeCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return Ok(result);
    }

    [HttpDelete("{employeeId:int}")]
    public async Task<IActionResult> Delete(int employeeId)
    {
        var ok = await _mediator.Send(new DeleteEmployeeCommand
        {
            EmployeeId = employeeId,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return ok ? Ok(new { message = "Empleado eliminado lógicamente." })
                  : BadRequest(new { message = "No se pudo eliminar el empleado." });
    }

    [HttpGet("{employeeId:int}")]
    public async Task<IActionResult> GetById(int employeeId)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery { EmployeeId = employeeId });
        if (result == null) return NotFound(new { message = "Empleado no encontrado." });
        return Ok(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] bool? isActive, [FromQuery] int? departmentId, [FromQuery] int? positionId)
    {
        var result = await _mediator.Send(new ListEmployeesQuery
        {
            IsActive = isActive,
            DepartmentId = departmentId,
            PositionId = positionId
        });
        return Ok(result);
    }

    // Combo para desplegable
    [HttpGet("select")]
    public async Task<IActionResult> Select([FromQuery] bool? isActive, [FromQuery] int? departmentId)
    {
        var result = await _mediator.Send(new ListEmployeeSelectQuery
        {
            IsActive = isActive,
            DepartmentId = departmentId
        });
        return Ok(result);
    }
}