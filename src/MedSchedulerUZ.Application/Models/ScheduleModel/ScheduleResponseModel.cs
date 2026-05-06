using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.ScheduleModel
{
    public class ScheduleResponseModel : BaseResponseModel
    {
        public Guid HospitalId { get; set; }
        public string HospitalName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public ScheduleStatus Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
