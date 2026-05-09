namespace MedSchedulerUZ.Application.Models.DepartmentModel
{
    public class DepartmentResponseModel : BaseResponseModel
    {
        public Guid HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string Name { get; set; }
        public int MinStaffRequired { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
