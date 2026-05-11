namespace MedSchedulerUZ.Application.Models.AttendanceModel
{
    public class ClockInModel
    {
        public Guid ShiftId { get; set; }
        public string QrToken { get; set; }
    }
}
