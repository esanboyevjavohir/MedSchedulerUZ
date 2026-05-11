namespace MedSchedulerUZ.Application.Models.AttendanceModel
{
    public class ClockOutModel
    {
        public Guid ShiftId { get; set; }
        public string QrToken { get; set; }
    }
}
