using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ShiftModel
{
    public class CreateShiftModel
    {
        public Guid ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public bool IsOnCall { get; set; } = false;
    }

    public class CreateShiftResponseModel : BaseResponseModel { }
}
