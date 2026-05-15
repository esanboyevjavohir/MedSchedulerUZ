namespace MedSchedulerUZ.Application.Models.CertificationModel
{
    public class AddCertificationModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentFileName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class AddCertificationResponseModel : BaseResponseModel { }
}
