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
        public async Task<bool> SendEmailAsync(string email, string otp)
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
                    Subject = "Your EduPress Verification Code",
                    Body = $"Dear {email}," +
                           "\nYou are using this email address to register on our website." +
                           $"\n\nYour verification code is {otp}." +
                           "\nPlease use it to complete your registration before it expires." +
                           "\n\nIf you didn't request this, please ignore this email." +
                           "\n\nThank you!",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
