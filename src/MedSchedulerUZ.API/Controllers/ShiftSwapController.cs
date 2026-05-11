using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.ShiftSwapModel;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class ShiftSwapController : ApiController
    {
        private readonly IShiftSwapService _shiftSwapService;

        public ShiftSwapController(IShiftSwapService shiftSwapService)
        {
            _shiftSwapService = shiftSwapService;
        }

        [HttpPost("request")]
        [Authorize(Roles = "Employee,DeptHead")]
        public async Task<IActionResult> RequestSwap([FromBody] RequestSwapModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _shiftSwapService.RequestSwapAsync(currentUserId, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{swapId}/accept")]
        [Authorize(Roles = "Employee,DeptHead")]
        public async Task<IActionResult> AcceptSwap(Guid swapId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _shiftSwapService.AcceptSwapAsync(swapId, currentUserId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{swapId}/approve")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> ApproveSwap(Guid swapId)
        {
            var approverId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _shiftSwapService.ApproveSwapAsync(swapId, approverId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{swapId}/assign/{acceptorId}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> AssignSwap(Guid swapId, Guid acceptorId)
        {
            var approverId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _shiftSwapService.AssignSwapAsync(swapId, acceptorId, approverId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _shiftSwapService.GetPendingAsync();
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

            var result = await _shiftSwapService.GetByUserIdAsync(userId);
            return Ok(result);
        }
    }
}
