using AutoMapper;
using MedSchedulerUZ.Application.Models.ScheduleModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class ScheduleMapping : Profile
    {
        public ScheduleMapping()
        {
            CreateMap<Schedule, ScheduleResponseModel>()
                .ForMember(dest => dest.HospitalName, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentName, opt => opt.Ignore());
            CreateMap<CreateScheduleModel, Schedule>();
        }
    }
}
