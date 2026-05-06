namespace MedSchedulerUZ.Application.Models.DepartmentModel
{
    public class UpdateDepartmentModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int MinStaffRequired { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateDepartmentResponseModel : BaseResponseModel { }
}
