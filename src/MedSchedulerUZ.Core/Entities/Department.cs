using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class Department : BaseEntity, IAuditedEntity // Bo'lim
    {
        public Guid HospitalId { get; set; }
        public string Name { get; set; }
        public int MinStaffRequired { get; set; } // Minimum kerakli xodim soni
        public bool IsActive { get; set; } = true; // Bo'lim xozir faolmi 

        // Navigation properties
        public Hospital Hospital { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Shift> Shifts { get; set; } // shu bo'limda tayinlangan barcha smenalar ro'yxati.
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
