namespace MedSchedulerUZ.Application.Models.DepartmentModel
{
    public class CreateDepartmentModel
    {
        public Guid HospitalId { get; set; }
        public string Name { get; set; }
        public int MinStaffRequired { get; set; }
    }

    public class CreateDepartmentResponseModel : BaseResponseModel { }
}
