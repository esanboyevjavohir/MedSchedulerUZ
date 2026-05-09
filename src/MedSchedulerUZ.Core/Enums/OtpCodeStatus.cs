namespace MedSchedulerUZ.Core.Enums
{
    public enum OtpCodeStatus
    {
        Unverified = 1, // hali tasdiqlanmagan
        Verified, // tasdiqlangan
        Expired, // muddati o'tdi
        Blocked // xodim kodni ko'p marta noto'g'ri kiritsa bloklanadi
    }
}
