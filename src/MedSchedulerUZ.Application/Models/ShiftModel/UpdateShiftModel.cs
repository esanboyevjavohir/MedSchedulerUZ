using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ShiftModel
{
    public class UpdateShiftModel 
    {
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public ShiftStatus Status { get; set; }
        public bool IsOnCall { get; set; }
    }

    public class UpdateShiftResponseModel : BaseResponseModel { }
}
