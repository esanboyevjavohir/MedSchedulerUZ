using AutoMapper;
using MedSchedulerUZ.Application.Models.ShiftModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class ShiftMapping : Profile
    {
        public ShiftMapping()
        {
            CreateMap<Shift, ShiftResponseModel>()
                .ForMember(dest => dest.UserFullName,
            opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.DepartmentName,
            opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<CreateShiftModel, Shift>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src =>
                    DateTime.Parse(src.StartTime).TimeOfDay))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src =>
                    DateTime.Parse(src.EndTime).TimeOfDay));
        }
    }
}
