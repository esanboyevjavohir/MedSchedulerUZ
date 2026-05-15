namespace MedSchedulerUZ.Application.Models.CertificationModel
{
    public class CertificationResponseModel : BaseResponseModel
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public string Name { get; set; }
        public string? DocumentFileName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsNotified { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        public bool IsExpiringSoon => ExpiryDate.HasValue &&
                                      ExpiryDate.Value > DateTime.UtcNow &&
                                      ExpiryDate.Value <= DateTime.UtcNow.AddDays(30);
    }
}
