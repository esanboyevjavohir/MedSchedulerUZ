namespace MedSchedulerUZ.Application.Models.SpecializationModel
{
    public class CreateSpecializationModel
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
    }

    public class CreateSpecializationResponseModel : BaseResponseModel { }
}
