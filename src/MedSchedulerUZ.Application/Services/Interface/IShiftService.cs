using MedSchedulerUZ.Application.Models.ShiftModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IShiftService
    {
        Task<ApiResult<string>> GetQrTokenAsync(Guid shiftId);
        Task<ApiResult<CreateShiftResponseModel>> CreateAsync(CreateShiftModel model);
        Task<ApiResult<UpdateShiftResponseModel>> UpdateAsync(Guid id, UpdateShiftModel model);
        Task<ApiResult<ShiftResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<ShiftResponseModel>>> GetAllAsync();
        Task<ApiResult<List<ShiftResponseModel>>> GetByUserIdAsync(Guid userId);
        Task<ApiResult<List<ShiftResponseModel>>> GetByScheduleIdAsync(Guid scheduleId);
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
