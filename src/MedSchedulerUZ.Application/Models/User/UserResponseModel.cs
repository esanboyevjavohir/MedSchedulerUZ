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
        public Guid? HospitalId { get; set; }    
        public string? HospitalName { get; set; } // (optional, foydali)
        public Guid? DepartmentId { get; set; }   
        public string? DepartmentName { get; set; } // (optional, foydali)
        public DateTime CreatedOn { get; set; }
    }
}
