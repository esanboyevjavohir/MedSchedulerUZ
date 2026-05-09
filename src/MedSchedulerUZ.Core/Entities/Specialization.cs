using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Core.Entities
{
    public class Specialization : BaseEntity // Mutaxassislik
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }   // "Kardiolog", "Jarroh"
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Department Department { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
