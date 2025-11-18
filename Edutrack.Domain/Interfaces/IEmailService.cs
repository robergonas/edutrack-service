namespace EduTrack.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}