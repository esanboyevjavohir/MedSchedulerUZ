namespace MedSchedulerUZ.Application.Models.User
{
    public class CreateUserModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public Guid HospitalId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid RoleId { get; set; }
        public Guid SpecializationId { get; set; }
    }

    public class CreateUserResponseModel : BaseResponseModel 
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
    }
}
