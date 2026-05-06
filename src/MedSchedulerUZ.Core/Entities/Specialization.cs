using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class Specialization : BaseEntity, IAuditedEntity // Mutaxassislik
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }   // "Kardiolog", "Jarroh"
        public string Code { get; set; }   // "CARDIO", "SURG"
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Department Department { get; set; }
        public ICollection<User> Users { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
