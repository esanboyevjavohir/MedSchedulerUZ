using AutoMapper;
using MedSchedulerUZ.Application.Models.ShiftSwapModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class ShiftSwapMapping : Profile
    {
        public ShiftSwapMapping()
        {
            CreateMap<ShiftSwap, ShiftSwapResponseModel>()
                .ForMember(dest => dest.RequesterFullName,
            opt => opt.MapFrom(src => src.Requester.FullName))
                .ForMember(dest => dest.AcceptorFullName,
            opt => opt.MapFrom(src => src.Acceptor != null ? src.Acceptor.FullName : null))
                .ForMember(dest => dest.ApproverFullName,
            opt => opt.MapFrom(src => src.Approver != null ? src.Approver.FullName : null))
                .ForMember(dest => dest.ShiftDate,
            opt => opt.MapFrom(src => src.Shift.ShiftDate));
        }
    }
}
