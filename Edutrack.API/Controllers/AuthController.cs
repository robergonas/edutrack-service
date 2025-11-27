using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduTrack.Application.Authentication.Commands;
using EduTrack.Infrastructure.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly PermissionService _permissionService;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {        
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Se ha enviado una nueva contraseña al correo registrado." });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUserCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Contraseña cambiada correctamente." });
    }

    //[HttpGet("permissions/{userId}")]
    //public async Task<IActionResult> GetPermissions(int userId)
    //{
    //    var permissions = await _permissionService.GetPermissionsByUserIdAsync(userId);
    //    return Ok(permissions);
    //}

}