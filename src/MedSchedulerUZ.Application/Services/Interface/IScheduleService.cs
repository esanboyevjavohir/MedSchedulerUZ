using MedSchedulerUZ.Application.Models.ScheduleModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IScheduleService
    {
        Task<ApiResult<CreateScheduleResponseModel>> CreateAsync(CreateScheduleModel model);
        Task<ApiResult<UpdateScheduleResponseModel>> UpdateAsync(Guid id, UpdateScheduleModel model);
        Task<ApiResult<ScheduleResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<ScheduleResponseModel>>> GetAllAsync();
        Task<ApiResult<List<ScheduleResponseModel>>> GetByDepartmentIdAsync(Guid departmentId);
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
