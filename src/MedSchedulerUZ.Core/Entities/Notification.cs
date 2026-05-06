using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Notification : BaseEntity, IAuditedEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
