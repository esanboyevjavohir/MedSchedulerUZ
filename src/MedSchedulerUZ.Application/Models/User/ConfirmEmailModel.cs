namespace MedSchedulerUZ.Application.Models.User
{
    public class ConfirmEmailModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailResponseModel
    {
        public bool Confirmed { get; set; }
    }
}
