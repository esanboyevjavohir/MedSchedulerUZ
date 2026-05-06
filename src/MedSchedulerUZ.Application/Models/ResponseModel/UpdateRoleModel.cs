using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ResponseModel
{
    public class UpdateRoleModel
    {
        public UserRole RoleType { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateRoleResponseModel : BaseResponseModel { }
}
