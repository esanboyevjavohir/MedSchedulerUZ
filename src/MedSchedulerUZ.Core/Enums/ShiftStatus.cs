namespace MedSchedulerUZ.Core.Enums
{
    public enum ShiftStatus
    {
        Scheduled = 1, // rejalashtirilgan
        Completed = 2, // tugallangan
        Missed = 3, // xodim kelmadi
        Swapped = 4, // boshqa xodim bilan almashtirildi
        Cancelled = 5
    }
}
