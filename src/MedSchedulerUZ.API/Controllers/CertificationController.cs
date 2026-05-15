using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models.CertificationModel;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class CertificationController : ApiController
    {
        private readonly ICertificationService _certificationService;

        public CertificationController(ICertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        [HttpPost]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Add([FromBody] AddCertificationModel model)
        {
            var result = await _certificationService.AddAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
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

            var result = await _certificationService.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _certificationService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
