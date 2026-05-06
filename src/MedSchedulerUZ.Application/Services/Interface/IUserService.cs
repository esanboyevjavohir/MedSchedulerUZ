using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Models.User;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IUserService
    {
        Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel model);
        Task<ApiResult<CreateUserResponseModel>> RegisterAsync(CreateUserModel model);
        Task<ApiResult<UserResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<bool>> SendOtpCode(Guid userId);
        Task<ApiResult<bool>> VerifyOtpCode(string code, Guid userId);
        Task<ApiResult<List<UserResponseModel>>> GetAllAsync();
        Task<ApiResult<bool>> DeleteUserAsync(Guid id);
    }
}
