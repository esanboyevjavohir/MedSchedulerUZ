using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class Notification : BaseEntity // Bildirishnoma
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } // bildirishnomaning to'liq matni
        public NotificationType Type { get; set; } // bildirishnoma turi
        public bool IsRead { get; set; } = false; //  xodim bildirishnomani o'qidimi yo'qmi?
        public DateTime CreatedOn { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
