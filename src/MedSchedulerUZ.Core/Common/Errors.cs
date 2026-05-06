namespace MedSchedulerUZ.Core.Common
{
    public record Errors(string code, string message)
    {
        public static Errors None = new Errors(string.Empty, string.Empty);

        public static Errors NullValue = new Errors("Error.NullValue", "Null value was provided");
        public static Errors DataBaseError = new Errors("Error.DataBaseError", "There is some problem with the DataBase");
        public static Errors BadRequest = new Errors("Error.BadRequest", "Somewhere the query didn't work");
        public static Errors NotFound = new Errors("Error.NotFound", "NotFound");
        public static Errors Unauthorized = new Errors("Error.Unauthorized", "Unauthorized");
        public static Errors Forbidden = new Errors("Error.Forbidden", "Forbidden");
        public static Errors Conflict = new Errors("Error.Conflict", "Conflict");
        public static Errors InternalServerError = new Errors("Error.InternalServerError", "Internal Server Error");
        public static Errors LoginFaild = new Errors("Error.LoginFaild", "Username or Password is incorrect");
        public static Errors TechnicalWorks = new Errors("Error.TechnicalWorks", "Ushbu Apida texnik ishlar olib borilmoqda");
        public static Errors DatabaseError = new Errors("Error.DatabaseError", "Database error");
        public static Errors InvalidOperation = new Errors("Error.InvalidOperation", "Invalid operation");
        public static Errors TimeSlotOverlap = new Errors("Error.TimeSlotOverlap", "Time slot overlaps with existing appointment");
        public static Errors ScheduleHasAppointments = new Errors("Error.ScheduleHasAppointments", "Schedule has appointments");
        public static Errors TimeSlotNotAvailable = new Errors("Error.TimeSlotNotAvailable", "Time slot is not available");
    }
}
