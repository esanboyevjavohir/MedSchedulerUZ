using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace MedSchedulerUZ.API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserModel loginUser)
        {
            var result = await _userService.LoginAsync(loginUser);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("register")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Register([FromBody] CreateUserModel model)
        {
            var currentRole = User.FindFirst(CustomClaimNames.Role)!.Value;
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);

            // DeptHead faqat Employee qo'sha oladi
            if (currentRole == UserRole.DeptHead.ToString() && model.RoleType != UserRole.Employee)
                return Forbid();

            // HospitalAdmin SuperAdmin qo'sha olmaydi
            if (currentRole == UserRole.HospitalAdmin.ToString() && model.RoleType == UserRole.SuperAdmin)
                return Forbid();

            var result = await _userService.RegisterAsync(model, currentRole, currentUserId);
            if (!result.Succedded) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("send-otp/{userId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Emailga otp code yuborish")]
        public async Task<IActionResult> SendOtpCode(Guid userId)
        {
            var result = await _userService.SendOtpCode(userId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Otp codeni tasdiqlash")]
        public async Task<IActionResult> VerifyOtpCode(string otpCode, Guid userId)
        {
            var result = await _userService.VerifyOtpCode(otpCode, userId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("profile-update")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _userService.UpdateProfileAsync(currentUserId, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _userService.ChangePasswordAsync(currentUserId, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(new {message = "Parolingiz muvaffaqiyatli o'zgartirildi. Iltimos, qayta login qiling!" });
        }

        [HttpGet("my-profile")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var result = await _userService.GetMeAsync(currentUserId);
            return Ok(result);
        }

        [HttpPost("resend-otp/{userId}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Otp codeni qayta yuborish")]
        public async Task<IActionResult> ResendOtpCode(Guid userId)
        {
            var result = await _userService.ResendOtpCode(userId);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Parol yoddan chiqqan xolatda emailga vaqtinchalik code yuborish")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _userService.ForgotPasswordAsync(model.Email);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Yangi parol qo'yish")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Access token muddati tugaganda yangilash uchun")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var result = await _userService.ValidateAndRefreshToken(model.Id, model.RefreshToken);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currentUserId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);
            var currentUserRole = User.FindFirst(CustomClaimNames.Role)!.Value;

            if (currentUserRole == UserRole.Employee.ToString() && id != currentUserId)
                return Forbid();

            var result = await _userService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetAllUser")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var role = User.FindFirst(CustomClaimNames.Role)!.Value;
            var userId = Guid.Parse(User.FindFirst(CustomClaimNames.Id)!.Value);

            var result = await _userService.GetAllAsync(role, userId);
            if (!result.Succedded) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUserRole = User.FindFirst(CustomClaimNames.Role)!.Value;
            var targetUser = await _userService.GetByIdAsync(id);

            if (!targetUser.Succedded)
                return NotFound(targetUser);

            // HospitalAdmin faqat DeptHead va Employee ni o'chira oladi
            if (currentUserRole == UserRole.HospitalAdmin.ToString() &&
                targetUser.Result.RoleType == "SuperAdmin")
                return Forbid();

            // DeptHead faqat Employee ni o'chira oladi
            if (currentUserRole == UserRole.DeptHead.ToString() &&
                targetUser.Result.RoleType != "Employee")
                return Forbid();

            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succedded)
                return NotFound(result);
            return Ok(result);
        }
    }
}
