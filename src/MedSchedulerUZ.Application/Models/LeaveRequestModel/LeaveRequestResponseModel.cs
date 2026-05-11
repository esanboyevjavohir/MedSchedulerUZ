using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.LeaveRequestModel
{
    public class LeaveRequestResponseModel : BaseResponseModel
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string? ApproverFullName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RespondedAt { get; set; }
    }
}
