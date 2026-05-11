namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string email, string subject);
        Task<bool> SendPasswordAsync(string email, string fullName, string password);
    }
}
