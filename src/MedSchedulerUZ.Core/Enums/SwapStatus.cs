namespace MedSchedulerUZ.Core.Enums
{
    public enum SwapStatus
    {
        Pending = 1, // so'rov yuborildi, kutilmoqda
        Accepted = 2, // boshqa xodim qabul qildi
        Rejected = 3, // rad etildi
        Approved = 4 // admin/menejer tasdiqladi, smena rasman almashtirildi
    }
}
