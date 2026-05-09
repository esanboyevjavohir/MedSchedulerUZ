using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class WorkHoursSummary : BaseEntity // Oylik ish vaqti xulosasi
    {
        public Guid UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double RegularHours { get; set; } = 0; // xodim shu oyda oddiy ish vaqtida ishlagan soatlar soni
        public double OvertimeHours { get; set; } = 0; // xodim shu oyda qo'shimcha (norma dan ortiq) ishlagan soatlar
        public double LeaveDays { get; set; } = 0; // xodim shu oyda ta'tilda bo'lgan kunlar soni
        public double SickDays { get; set; } = 0; // xodim shu oyda kasal bo'lib ishga kelmagan kunlar soni
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
