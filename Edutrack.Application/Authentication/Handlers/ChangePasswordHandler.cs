using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using EduTrack.Application.Authentication.Commands;
using EduTrack.Persistence;
using EduTrack.Domain.Interfaces;
public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly EduTrackDbContext _context;    
    public ChangePasswordHandler(EduTrackDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        // Validar clave anterior
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new Exception("La clave anterior no es correcta");

        // Validar nueva clave (ejemplo simple)
        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8)
            throw new Exception("La nueva clave no cumple con los requisitos de seguridad");

        // Guardar nueva clave
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);        

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;

    }    
}

