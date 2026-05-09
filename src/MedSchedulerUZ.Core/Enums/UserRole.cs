namespace MedSchedulerUZ.Core.Enums
{
    public enum UserRole
    {
        SuperAdmin = 1, // barcha kasalxona va filiallarni boshqaradi
        HospitalAdmin = 2, // faqat o'z kasalxonasini boshqaradi
        DeptHead = 3, // faqat o'z bo'limini boshqaradi
        Employee = 4 // faqat o'z jadvalini ko'radi
    }
}
