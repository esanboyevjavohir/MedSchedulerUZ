using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ScheduleModel
{
    public class UpdateScheduleModel 
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public ScheduleStatus Status { get; set; }
    }

    public class UpdateScheduleResponseModel : BaseResponseModel { }
}
