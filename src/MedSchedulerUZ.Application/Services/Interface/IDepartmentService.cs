using MedSchedulerUZ.Application.Models.DepartmentModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IDepartmentService
    {
        Task<ApiResult<CreateDepartmentResponseModel>> CreateAsync(CreateDepartmentModel model);
        Task<ApiResult<UpdateDepartmentResponseModel>> UpdateAsync(Guid id, UpdateDepartmentModel model);
        Task<ApiResult<DepartmentResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<DepartmentResponseModel>>> GetAllAsync();
        Task<ApiResult<List<DepartmentResponseModel>>> GetByHospitalIdAsync(Guid hospitalId);
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
