using EduTrack.Application.Authentication.Commands;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var command = new CreateDepartmentCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        };

        var created = await _mediator.Send(command);
        return Ok(created);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateDepartmentDto dto)
    {
        var command = new UpdateDepartmentCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        };

        var updated = await _mediator.Send(command);
        return Ok(updated);
    }
    
    [HttpDelete("{departmentId:int}")]
    public async Task<IActionResult> Delete(int departmentId)
    {
        var command = new DeleteDepartmentCommand
        {
            DepartmentId = departmentId,
            CurrentUserName = User?.Identity?.Name ?? "system"
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { message = "Departamento eliminado correctamente." })
                      : BadRequest(new { message = "No se pudo eliminar el departamento." });
    }
    
    [HttpGet("{departmentId:int}")]
    public async Task<IActionResult> GetById(int departmentId)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery { DepartmentId = departmentId });
        if (result == null) return NotFound(new { message = "Departamento no encontrado." });
        return Ok(result);
    }
    
    [HttpGet("List")]
    public async Task<IActionResult> List([FromQuery] bool? isActive)
    {
        var result = await _mediator.Send(new ListDepartmentsQuery { IsActive = isActive });

        return Ok(new { 
        success=true,
        data=result
        });
    }
}