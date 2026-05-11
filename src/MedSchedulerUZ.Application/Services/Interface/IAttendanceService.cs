using MedSchedulerUZ.Application.Models.AttendanceModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IAttendanceService
    {
        Task<ApiResult<AttendanceResponseModel>> ClockInAsync(Guid userId, ClockInModel model);
        Task<ApiResult<AttendanceResponseModel>> ClockOutAsync(Guid userId, ClockOutModel model);
        Task<ApiResult<AttendanceResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<AttendanceResponseModel>>> GetByUserIdAsync(Guid userId);
        Task<ApiResult<List<AttendanceResponseModel>>> GetByShiftIdAsync(Guid shiftId);
    }
}
