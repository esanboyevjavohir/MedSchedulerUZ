using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSchedulerUZ.Core.Entities
{
    public class Schedule : BaseEntity, IAuditedEntity // Ish jadvali
    {
        public Guid HospitalId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public ScheduleStatus Status { get; set; } = ScheduleStatus.Draft;
        public Guid CreatedBy { get; set; } // bu jadval kim tomonidan tuzilgan — HospitalAdmin yoki DeptHead
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public Hospital Hospital { get; set; }
        public Department Department { get; set; }

        [ForeignKey("CreatedBy")]
        public User Creator { get; set; }
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>(); //  Ish jadvali aslida
                                                                            //  smenalar yig'indisidan iborat
    }
}
