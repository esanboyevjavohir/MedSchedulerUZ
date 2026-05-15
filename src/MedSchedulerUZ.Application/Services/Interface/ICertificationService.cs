using MedSchedulerUZ.Application.Models.CertificationModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface ICertificationService
    {
        Task<ApiResult<AddCertificationResponseModel>> AddAsync(AddCertificationModel model);
        Task<ApiResult<List<CertificationResponseModel>>> GetByUserIdAsync(Guid userId);
        Task<ApiResult<bool>> DeleteAsync(Guid id);
        Task CheckExpiringAsync();
    }
}
