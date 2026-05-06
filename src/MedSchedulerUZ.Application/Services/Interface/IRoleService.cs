using MedSchedulerUZ.Application.Models.ResponseModel;
using MedSchedulerUZ.Application.Models;

namespace MedSchedulerUZ.Application.Services.Interface
{
    public interface IRoleService
    {
        Task<ApiResult<CreateRoleResponseModel>> CreateAsync(CreateRoleModel model);
        Task<ApiResult<UpdateRoleResponseModel>> UpdateAsync(Guid id, UpdateRoleModel model);
        Task<ApiResult<RoleResponseModel>> GetByIdAsync(Guid id);
        Task<ApiResult<List<RoleResponseModel>>> GetAllAsync();
        Task<ApiResult<bool>> DeleteAsync(Guid id);
    }
}
