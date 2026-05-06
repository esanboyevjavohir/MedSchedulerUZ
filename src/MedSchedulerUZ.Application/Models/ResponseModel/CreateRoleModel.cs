using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ResponseModel
{
    public class CreateRoleModel
    {
        public UserRole RoleType { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; } // JSON formatda
    }

    public class CreateRoleResponseModel : BaseResponseModel { }
}
