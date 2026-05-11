namespace MedSchedulerUZ.Application.Models.ShiftSwapModel
{
    public class RequestSwapModel
    {
        public Guid ShiftId { get; set; }
        public string Reason { get; set; }
    }

    public class RequestSwapResponseModel : BaseResponseModel { }

    public class AcceptSwapResponseModel : BaseResponseModel { }

    public class ApproveSwapResponseModel : BaseResponseModel { }
}
