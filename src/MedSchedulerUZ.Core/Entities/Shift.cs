using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Shift : BaseEntity, IAuditedEntity // Smena
    {
        public Guid ScheduleId { get; set; } // Jadval Id si
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime ShiftDate { get; set; } // Smena kuni
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public ShiftStatus Status { get; set; } = ShiftStatus.Scheduled; // smena statusi
        public bool IsOnCall { get; set; } = false;
        public string? QrToken { get; set; } // xodim davomat uchun scan qiladigan unikal QR kod tokeni
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public Schedule Schedule { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<ShiftSwap> ShiftSwaps { get; set; } = new List<ShiftSwap>();
    }
}
