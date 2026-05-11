using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.LeaveRequestModel
{
    public class CreateLeaveRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; }
    }

    public class CreateLeaveRequestResponseModel : BaseResponseModel { }
}
