using MedSchedulerUZ.Application.Models.HospitalModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IHospitalService
    {
        Task<ApiResult<CreateHospitalResponseModel>> CreateAsync(CreateHospitalModel model);
        Task<ApiResult<UpdateHospitalResponseModel>> UpdateAsync(Guid id, UpdateHospitalModel model);
        Task<ApiResult<HospitalResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<HospitalResponseModel>>> GetAllAsync();
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
