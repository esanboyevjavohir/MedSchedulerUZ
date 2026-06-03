using MedSchedulerUZ.Application.Models.LeaveRequestModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface ILeaveRequestService
    {
        Task<ApiResult<CreateLeaveRequestResponseModel>> CreateAsync(Guid userId, CreateLeaveRequestModel model);
        Task<ApiResult<UpdateLeaveRequestResponseModel>> RespondAsync(Guid id, Guid approverId, UpdateLeaveRequestModel model);
        Task<ApiResult<LeaveRequestResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<LeaveRequestResponseModel>>> GetByUserIdAsync(Guid userId);
        Task<ApiResult<List<LeaveRequestResponseModel>>> GetAllAsync();
        Task<ApiResult<List<LeaveRequestResponseModel>>> GetPendingAsync();
    }
}
