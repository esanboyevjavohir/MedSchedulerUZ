namespace MedSchedulerUZ.Application.Models.SpecializationModel
{
    public class SpecializationResponseModel : BaseResponseModel
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public Guid DepartmentId { get; set; }      
        public string DepartmentName { get; set; }
        public string? HospitalName { get; set; }
    }
}
