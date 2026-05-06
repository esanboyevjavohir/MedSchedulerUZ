namespace MedSchedulerUZ.Application.Models.User
{
    public class UserResponseModel : BaseResponseModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public string RoleName { get; set; } = null!;
        public string SpecializationName { get; set; } = null!;
    }
}
