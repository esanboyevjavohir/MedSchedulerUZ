using MedSchedulerUZ.Application.Models.NotificationModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface INotificationService
    {
        Task<ApiResult<CreateNotificationResponseModel>> CreateAsync(CreateNotificationModel model);
        Task<ApiResult<bool>> MarkAsReadAsync(Guid id);
        Task<ApiResult<bool>> MarkAllAsReadAsync(Guid userId);
        Task<ApiResult<List<NotificationResponseModel>>> GetByUserIdAsync(Guid userId);
        Task<ApiResult<List<NotificationResponseModel>>> GetUnreadByUserIdAsync(Guid userId);
    }
}
