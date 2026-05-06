using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Hospital : BaseEntity, IAuditedEntity // Tibbiyot muassasasi
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public HospitalType Type { get; set; } // "Central", "Branch"
        public bool IsActive { get; set; } = true; // Kasalxona tizimda faolmi

        // Navigation properties
        public ICollection<Department> Departments { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Schedule> Schedules { get; set; } // shu kasalxona uchun tuzilgan barcha jadvallar
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
