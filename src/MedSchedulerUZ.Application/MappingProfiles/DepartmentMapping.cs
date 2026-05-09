using AutoMapper;
using MedSchedulerUZ.Application.Models.DepartmentModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class DepartmentMapping : Profile
    {
        public DepartmentMapping()
        {
            CreateMap<Department, DepartmentResponseModel>()
                .ForMember(dest => dest.HospitalName, 
                    opt => opt.MapFrom(src => src.Hospital.Name)); 

            CreateMap<CreateDepartmentModel, Department>();
        }
    }
}
