namespace MedSchedulerUZ.Core.Enums
{
    public enum ScheduleStatus
    {
        Draft = 1, // hali tayyorlanmoqda, xodimlar ko'rmaydi
        Published = 2, // nashr etilgan, xodimlar o'z smenalarini ko'ra oladi
        Archived = 3 // o'tib ketgan jadval, arxivda saqlanadi
    }
}
