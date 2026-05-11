using MedSchedulerUZ.Application.Models.ShiftSwapModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IShiftSwapService
    {
        Task<ApiResult<RequestSwapResponseModel>> RequestSwapAsync(Guid requesterId, RequestSwapModel model);
        Task<ApiResult<AcceptSwapResponseModel>> AcceptSwapAsync(Guid swapId, Guid acceptorId);
        Task<ApiResult<ApproveSwapResponseModel>> ApproveSwapAsync(Guid swapId, Guid approverId);
        Task<ApiResult<ApproveSwapResponseModel>> AssignSwapAsync(Guid swapId, Guid acceptorId, Guid approverId);
        Task<ApiResult<List<ShiftSwapResponseModel>>> GetPendingAsync();
        Task<ApiResult<List<ShiftSwapResponseModel>>> GetByUserIdAsync(Guid userId);
    }
}
