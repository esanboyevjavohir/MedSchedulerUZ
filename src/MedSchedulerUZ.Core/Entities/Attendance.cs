using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Attendance : BaseEntity, IAuditedEntity // Davomat
    {
        public Guid UserId { get; set; }
        public Guid ShiftId { get; set; } // Smena Idsi
        public DateTime? ClockIn { get; set; } // Kirish vaqti
        public DateTime? ClockOut { get; set; } // Chiqish vaqti
        public AttendanceStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Shift Shift { get; set; }
    }
}
