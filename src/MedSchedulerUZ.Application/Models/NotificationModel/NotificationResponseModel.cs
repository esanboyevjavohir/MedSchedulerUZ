using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.NotificationModel
{
    public class NotificationResponseModel : BaseResponseModel
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
