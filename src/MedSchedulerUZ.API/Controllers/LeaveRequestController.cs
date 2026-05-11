using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.LeaveRequestModel;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class LeaveRequestController : ApiController
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee,DeptHead")]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _leaveRequestService.CreateAsync(currentUserId, model);
            return Ok(result);
        }

        [HttpPut("{id}/respond")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Respond(Guid id, [FromBody] UpdateLeaveRequestModel model)
        {
            var approverId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _leaveRequestService.RespondAsync(id, approverId, model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _leaveRequestService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var currentUserRole = User.FindFirst(CustomClaimNames.Role)!.Value;

            if (currentUserRole == UserRole.Employee.ToString() && userId != currentUserId)
                return Forbid();

            var result = await _leaveRequestService.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _leaveRequestService.GetPendingAsync();
            return Ok(result);
        }
    }
}
