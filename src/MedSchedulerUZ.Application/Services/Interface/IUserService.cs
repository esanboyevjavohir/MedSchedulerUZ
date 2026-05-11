using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Models.User;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IUserService
    {
        Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model);
        Task<ApiResult<CreateUserResponseModel>> RegisterAsync(CreateUserModel model);
        Task<ApiResult<UserResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<UserResponseModel>> GetMeAsync(Guid currentUserId);
        Task<ApiResult<bool>> SendOtpCode(Guid userId);
        Task<ApiResult<bool>> ResendOtpCode(Guid userId);
        Task<ApiResult<bool>> VerifyOtpCode(string code, Guid userId);
        Task<ApiResult<TokenResponseModel>> ValidateAndRefreshToken(Guid id, string refreshToken);
        Task<ApiResult<bool>> ForgotPasswordAsync(string email);
        Task<ApiResult<bool>> ResetPasswordAsync(ResetPasswordModel model);
        Task<ApiResult<bool>> ChangePasswordAsync(Guid userId, ChangePasswordModel model);
        Task<ApiResult<bool>> UpdateProfileAsync(Guid currentUserId, UpdateProfileModel model);
        Task<ApiResult<List<UserResponseModel>>> GetAllAsync();
        Task<ApiResult<bool>> DeleteUserAsync(Guid id);
    }
}
