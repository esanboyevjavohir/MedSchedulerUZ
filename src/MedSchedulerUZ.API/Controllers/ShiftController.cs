using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.ShiftModel;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class ShiftController : ApiController
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet("{id}/qr-token")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetQrToken(Guid id)
        {
            var result = await _shiftService.GetQrTokenAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateShiftModel model)
        {
            var result = await _shiftService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("auto-generate")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> AutoGenerate([FromBody] AutoGenerateShiftModel model)
        {
            var result = await _shiftService.AutoGenerateAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShiftModel model)
        {
            var result = await _shiftService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _shiftService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _shiftService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var currentUserRole = User.FindFirst(CustomClaimNames.Role)!.Value;

            if (currentUserRole == UserRole.Employee.ToString() && userId != currentUserId)
                return Forbid();

            var result = await _shiftService.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("schedule/{scheduleId}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetBySchedule(Guid scheduleId)
        {
            var result = await _shiftService.GetByScheduleIdAsync(scheduleId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _shiftService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result);
        }
    }
}
