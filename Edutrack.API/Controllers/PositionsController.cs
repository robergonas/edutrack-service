using EduTrack.Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/positions")]
public class PositionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crear una nueva posición
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePositionDto dto)
    {
        var result = await _mediator.Send(new CreatePositionCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return Ok(result);
    }

    /// <summary>
    /// Actualizar una posición existente
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePositionDto dto)
    {
        var result = await _mediator.Send(new UpdatePositionCommand
        {
            Dto = dto,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return Ok(result);
    }

    /// <summary>
    /// Eliminar una posición (borrado físico, ya que la tabla no tiene IsActive)
    /// </summary>
    [HttpDelete("{positionId:int}")]
    public async Task<IActionResult> Delete(int positionId)
    {
        var ok = await _mediator.Send(new DeletePositionCommand
        {
            PositionId = positionId,
            CurrentUserName = User?.Identity?.Name ?? "system"
        });
        return ok ? Ok(new { message = "Posición eliminada correctamente." })
                  : BadRequest(new { message = "No se pudo eliminar la posición." });
    }

    /// <summary>
    /// Obtener una posición por ID
    /// </summary>
    [HttpGet("{positionId:int}")]
    public async Task<IActionResult> GetById(int positionId)
    {
        var result = await _mediator.Send(new GetPositionByIdQuery { PositionId = positionId });
        if (result == null) return NotFound(new { message = "Posición no encontrada." });
        return Ok(result);
    }

    /// <summary>
    /// Listar todas las posiciones con detalle
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var result = await _mediator.Send(new ListPositionsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Listado reducido para combos desplegables
    /// </summary>
    [HttpGet("select")]
    public async Task<IActionResult> Select()
    {
        var result = await _mediator.Send(new ListPositionSelectQuery());
        return Ok(result);
    }
}