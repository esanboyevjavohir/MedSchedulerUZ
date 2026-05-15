using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class User : BaseEntity, IAuditedEntity
    {
        public Guid? HospitalId { get; set; }
        public Guid? DepartmentId { get; set; }
        public UserRole RoleType { get; set; }
        public Guid? SpecializationId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool MustChangePassword { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public Hospital Hospital { get; set; }
        public Department? Department { get; set; }
        public Specialization? Specialization { get; set; }
        public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
        public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<WorkHoursSummary> WorkHoursSummaries { get; set; } = new List<WorkHoursSummary>();
    }
}
