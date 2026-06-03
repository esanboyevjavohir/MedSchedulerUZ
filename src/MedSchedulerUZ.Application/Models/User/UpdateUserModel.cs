using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.User
{
    public class UpdateUserModel
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? HospitalId { get; set; }     
        public Guid? DepartmentId { get; set; } 
        public Guid? SpecializationId { get; set; } 
        public UserRole? RoleType { get; set; }   
        public bool? IsActive { get; set; }
    }
}
