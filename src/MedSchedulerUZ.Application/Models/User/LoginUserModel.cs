namespace MedSchedulerUZ.Application.Models.User
{
    public class LoginUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel : BaseResponseModel
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool MustChangePassword { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime RefreshTokenExpireAt { get; set; }
    }
}
