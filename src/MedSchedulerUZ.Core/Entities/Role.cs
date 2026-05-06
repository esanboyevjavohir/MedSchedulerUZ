using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Role : BaseEntity, IAuditedEntity
    {
        public UserRole RoleType { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
