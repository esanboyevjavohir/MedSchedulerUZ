namespace MedSchedulerUZ.Application.Models.SpecializationModel
{
    public class CreateSpecializationModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CreateSpecializationResponseModel : BaseResponseModel { }
}
