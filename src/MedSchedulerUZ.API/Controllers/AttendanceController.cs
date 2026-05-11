using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.AttendanceModel;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class AttendanceController : ApiController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("clock-in")]
        [Authorize(Roles = "Employee,DeptHead")]
        public async Task<IActionResult> ClockIn([FromBody] ClockInModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _attendanceService.ClockInAsync(currentUserId, model);
            return Ok(result);
        }

        [HttpPost("clock-out")]
        [Authorize(Roles = "Employee,DeptHead")]
        public async Task<IActionResult> ClockOut([FromBody] ClockOutModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _attendanceService.ClockOutAsync(currentUserId, model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _attendanceService.GetByIdAsync(id);
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

            var result = await _attendanceService.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("shift/{shiftId}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByShiftId(Guid shiftId)
        {
            var result = await _attendanceService.GetByShiftIdAsync(shiftId);
            return Ok(result);
        }
    }
}
