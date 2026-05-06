using MedSchedulerUZ.Application.Models.SpecializationModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface ISpecializationService
    {
        Task<ApiResult<CreateSpecializationResponseModel>> CreateAsync(CreateSpecializationModel model);
        Task<ApiResult<UpdateSpecializationResponseModel>> UpdateAsync(Guid id, UpdateSpecializationModel model);
        Task<ApiResult<SpecializationResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<SpecializationResponseModel>>> GetAllAsync();
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
