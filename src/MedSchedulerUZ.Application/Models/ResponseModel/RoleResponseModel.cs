using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ResponseModel
{
    public class RoleResponseModel : BaseResponseModel
    {
        public UserRole RoleType { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
