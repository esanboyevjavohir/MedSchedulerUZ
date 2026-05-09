namespace MedSchedulerUZ.Core.Enums
{
    public enum NotificationType
    {
        ScheduleChange = 1, // jadval o'zgarganda
        CertExpiry = 2, // sertifikat muddati tugashiga 30 kun qolganda
        ShiftSwap = 3, // smena almashish so'rovi kelganda
        LeaveStatus = 4 // ta'til so'rovi tasdiqlangan yoki rad etilganda
    }
}
