using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLoginAsync(LoginUserModel loginUser)
        {
            var result = await _userService.LoginAsync(loginUser);

            if (!result.Succedded)
            {
                if (result.Errors.Contains("User not found"))
                    return NotFound(result);
                if (result.Errors.Contains("Invalid password"))
                    return Unauthorized(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<ActionResult<CreateUserResponseModel>> UserSignUpAsync(
            [FromForm] CreateUserModel createUserModel)
        {
            var create = await _userService.RegisterAsync(createUserModel);
            if (!create.Succedded)
                return BadRequest(create);

            return Created("", create);
        }

        [HttpPost("SendOtpCode/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtpCodeAsync(Guid userId)
        {
            var result = await _userService.SendOtpCode(userId);

            if (!result.Succedded)
            {
                if (result.Errors.Contains("User not found"))
                    return NotFound(result);

                if (result.Errors.Contains("Failed to send OTP email"))
                    return StatusCode(500, result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("VerifyOtpCode")]
        public async Task<ApiResult<bool>> VerifyOtpCodeAsync(string otpCode, Guid userId)
        {
            var result = await _userService.VerifyOtpCode(otpCode, userId);
            return result;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var res = await _userService.GetByIdAsync(id);
                return res == null ? NotFound() : Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var res = await _userService.GetAllAsync();
                return res == null ? NotFound() : Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var res = await _userService.DeleteUserAsync(id);
                return res == null ? NotFound() : Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
