namespace MedSchedulerUZ.Application.Models.User
{
    public class UserSettings
    {
        public int OtpExpirationTimeInSeconds { get; set; } = 240;
        public int OtpResendTimeInSeconds { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 3;
    }
}
