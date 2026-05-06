using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Application.Models.HospitalModel
{
    public class HospitalResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public HospitalType Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
