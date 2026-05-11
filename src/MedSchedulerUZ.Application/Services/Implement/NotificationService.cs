using AutoMapper;
using MedSchedulerUZ.Application.Models.NotificationModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public NotificationService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateNotificationResponseModel>> CreateAsync(CreateNotificationModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId && u.IsActive);
            if (user is null)
                return ApiResult<CreateNotificationResponseModel>.Failure(["Xodim topilmadi"]);

            var notification = new Notification
            {
                UserId = model.UserId,
                Message = model.Message,
                Type = model.Type,
                IsRead = false,
                CreatedOn = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return ApiResult<CreateNotificationResponseModel>.Success(
                new CreateNotificationResponseModel { Id = notification.Id });
        }

        public async Task<ApiResult<bool>> MarkAsReadAsync(Guid id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification is null)
                return ApiResult<bool>.Failure(["Bildirishnoma topilmadi"]);

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<bool>> MarkAllAsReadAsync(Guid userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
                notification.IsRead = true;

            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<List<NotificationResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();

            return ApiResult<List<NotificationResponseModel>>.Success(
                _mapper.Map<List<NotificationResponseModel>>(notifications));
        }

        public async Task<ApiResult<List<NotificationResponseModel>>> GetUnreadByUserIdAsync(Guid userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();

            return ApiResult<List<NotificationResponseModel>>.Success(
                _mapper.Map<List<NotificationResponseModel>>(notifications));
        }
    }
}
