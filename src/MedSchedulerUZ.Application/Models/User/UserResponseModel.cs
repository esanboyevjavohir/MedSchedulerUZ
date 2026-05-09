namespace MedSchedulerUZ.Application.Models.User
{
    public class UserResponseModel : BaseResponseModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string RoleType { get; set; } = null!;
        public string? SpecializationName { get; set; }
    }
}
