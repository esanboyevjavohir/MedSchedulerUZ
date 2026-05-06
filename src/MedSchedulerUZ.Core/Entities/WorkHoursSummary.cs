using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class WorkHoursSummary : BaseEntity, IAuditedEntity
    {
        public Guid UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double RegularHours { get; set; } = 0;
        public double OvertimeHours { get; set; } = 0;
        public double LeaveDays { get; set; } = 0;
        public double SickDays { get; set; } = 0;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
