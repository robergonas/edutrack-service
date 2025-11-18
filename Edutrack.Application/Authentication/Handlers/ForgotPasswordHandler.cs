using Edutrack.Application.Common.Exceptions;
using EduTrack.Application.Authentication.Commands;
using EduTrack.Domain.Interfaces;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Application.Authentication.Handlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly EduTrackDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IRedisCacheService _rediscache;

        public ForgotPasswordHandler(EduTrackDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == request.Username && u.Email == request.Email, cancellationToken);

            if (user == null)
                throw new NotFoundException("Usuario o correo no válido.");
            
            //var token =Guid.NewGuid().ToString("N");
            //var rediskey=$"password-reset:{user.UserId}:{token}";
            //await _rediscache.SetAsync(rediskey, "valid", TimeSpan.FromMinutes(15));

            var newPassword = GenerateSecurePassword();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(new EmailMessage
            {
                Subject = "Recuperación de contraseña",
                Body = $"Hola {user.Username}, tu nueva contraseña temporal es: <strong>{newPassword}</strong>",
                To = new[] { user.Email }
            });

            return Unit.Value;
        }

        private string GenerateSecurePassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specials = "!@#$%^&*()-_=+[]{}";

            var random = new Random();

            // Garantizar al menos uno de cada tipo
            var passwordChars = new List<char>
            {
                upper[random.Next(upper.Length)],
                digits[random.Next(digits.Length)],
                specials[random.Next(specials.Length)]
            };

            // Completar hasta 10 caracteres con mezcla de todos
            string allChars = upper + lower + digits + specials;
            while (passwordChars.Count < 10)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            // Mezclar para evitar patrón predecible
            return new string(passwordChars.OrderBy(_ => random.Next()).ToArray());
        }

    }

}
