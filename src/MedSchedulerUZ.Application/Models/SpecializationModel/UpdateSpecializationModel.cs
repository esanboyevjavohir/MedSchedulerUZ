namespace MedSchedulerUZ.Application.Models.SpecializationModel
{
    public class UpdateSpecializationModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateSpecializationResponseModel : BaseResponseModel { }
}
