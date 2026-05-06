using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ShiftModel
{
    public class ShiftResponseModel : BaseResponseModel
    {
        public Guid ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public ShiftStatus Status { get; set; }
        public bool IsOnCall { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
