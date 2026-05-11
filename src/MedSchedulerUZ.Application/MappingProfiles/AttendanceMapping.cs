using AutoMapper;
using MedSchedulerUZ.Application.Models.AttendanceModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class AttendanceMapping : Profile
    {
        public AttendanceMapping()
        {
            CreateMap<Attendance, AttendanceResponseModel>()
                .ForMember(dest => dest.UserFullName,
            opt => opt.MapFrom(src => src.User.FullName));
        }
    }
}
