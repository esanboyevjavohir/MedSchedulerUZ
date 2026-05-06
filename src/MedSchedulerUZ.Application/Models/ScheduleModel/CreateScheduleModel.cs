namespace MedSchedulerUZ.Application.Models.ScheduleModel
{
    public class CreateScheduleModel
    {
        public Guid HospitalId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class CreateScheduleResponseModel : BaseResponseModel { }
}
