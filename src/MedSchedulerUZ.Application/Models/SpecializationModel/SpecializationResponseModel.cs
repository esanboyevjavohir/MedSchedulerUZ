namespace MedSchedulerUZ.Application.Models.SpecializationModel
{
    public class SpecializationResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
