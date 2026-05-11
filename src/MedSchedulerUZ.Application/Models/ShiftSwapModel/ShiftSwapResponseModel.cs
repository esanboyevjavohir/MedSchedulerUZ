using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ShiftSwapModel
{
    public class ShiftSwapResponseModel : BaseResponseModel
    {
        public Guid RequesterId { get; set; }
        public string RequesterFullName { get; set; }
        public Guid? AcceptorId { get; set; }
        public string? AcceptorFullName { get; set; }
        public Guid ShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        public SwapStatus Status { get; set; }
        public string Reason { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string? ApproverFullName { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
