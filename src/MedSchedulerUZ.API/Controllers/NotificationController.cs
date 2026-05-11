using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.NotificationModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateNotificationModel model)
        {
            var result = await _notificationService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            return Ok(result);
        }

        [HttpPut("read-all")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _notificationService.MarkAllAsReadAsync(currentUserId);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _notificationService.GetByUserIdAsync(currentUserId);
            return Ok(result);
        }

        [HttpGet("my/unread")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetMyUnreadNotifications()
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _notificationService.GetUnreadByUserIdAsync(currentUserId);
            return Ok(result);
        }
    }
}
