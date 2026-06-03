namespace MedSchedulerUZ.Application.Models.ShiftModel
{
    public class AutoGenerateShiftModel
    {
        public Guid ScheduleId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime WeekStart { get; set; } // Dushanbadan boshlanadi
    }

    public class AutoGenerateShiftResponseModel
    {
        public int CreatedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> Warnings { get; set; } = new();
    }
}
