using MedSchedulerUZ.Application.Email;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        private async Task<bool> SendAsync(string toEmail, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
                {
                    EnableSsl = _emailConfig.EnableSsl,
                    Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password)
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailConfig.DefaultFromEmail, _emailConfig.DefaultFromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendOtpAsync(string email, string otpCode)
        {
            var subject = "MedSchedulerUZ - Tasdiqlash kodi";
            var body = $"Hurmatli foydalanuvchi,\n\n" +
                       $"Sizning tasdiqlash kodingiz: {otpCode}\n\n" +
                       $"Kod 5 daqiqa davomida amal qiladi.\n\n" +
                       $"Agar siz bu so'rovni yubormagan bo'lsangiz, ushbu xatni e'tiborsiz qoldiring.";
            return await SendAsync(email, subject, body);
        }

        public async Task<bool> SendPasswordAsync(string email, string fullName, string password)
        {
            var subject = "MedSchedulerUZ - Tizimga kirish ma'lumotlari";
            var body = $"Hurmatli {fullName},\n\n" +
                       $"Siz MedSchedulerUZ tizimiga qo'shildingiz.\n\n" +
                       $"Email: {email}\n" +
                       $"Parol: {password}\n\n" +
                       $"Iltimos, tizimga kirgandan so'ng parolingizni o'zgartiring.\n\n" +
                       $"MedSchedulerUZ jamoasi";
            return await SendAsync(email, subject, body);
        }

        public async Task<bool> SendResetPasswordAsync(string email, string fullName, string tempPassword)
        {
            var subject = "MedSchedulerUZ - Parolni tiklash";
            var body = $"Hurmatli {fullName},\n\n" +
                       $"Parolni tiklash uchun so'rov qabul qilindi.\n\n" +
                       $"Vaqtinchalik parol: {tempPassword}\n\n" +
                       $"Bu parol 10 daqiqa davomida amal qiladi.\n" +
                       $"Tizimga kirib yangi parol o'rnating.\n\n" +
                       $"Agar siz bu so'rovni yubormagan bo'lsangiz, ushbu xatni e'tiborsiz qoldiring.\n\n" +
                       $"MedSchedulerUZ jamoasi";
            return await SendAsync(email, subject, body);
        }
    }
}
