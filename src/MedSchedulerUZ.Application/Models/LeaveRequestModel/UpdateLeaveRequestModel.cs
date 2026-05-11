using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.LeaveRequestModel
{
    public class UpdateLeaveRequestModel
    {
        public LeaveStatus Status { get; set; }
    }

    public class UpdateLeaveRequestResponseModel : BaseResponseModel { }
}
