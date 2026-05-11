using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.AttendanceModel
{
    public class AttendanceResponseModel : BaseResponseModel
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid ShiftId { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
