using AutoMapper;
using MedSchedulerUZ.Application.Models.LeaveRequestModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class LeaveRequestMapping : Profile
    {
        public LeaveRequestMapping()
        {
            CreateMap<LeaveRequest, LeaveRequestResponseModel>()
                .ForMember(dest => dest.UserFullName,
            opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ApproverFullName,
            opt => opt.MapFrom(src => src.Approver != null ? src.Approver.FullName : null));
        }
    }
}
