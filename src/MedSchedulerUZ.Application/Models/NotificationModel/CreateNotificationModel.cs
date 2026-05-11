using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.NotificationModel
{
    public class CreateNotificationModel
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }

    public class CreateNotificationResponseModel : BaseResponseModel { }
}
