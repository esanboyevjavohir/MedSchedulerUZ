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
                .ForMember(dest => dest.HospitalName, 
                    opt => opt.MapFrom(src => src.Hospital.Name))
                .ForMember(dest => dest.DepartmentName, 
                    opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<CreateScheduleModel, Schedule>();
        }
    }
}
