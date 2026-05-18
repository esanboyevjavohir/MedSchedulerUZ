using AutoMapper;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // User -> UserResponseModel
            CreateMap<User, UserResponseModel>()
                .ForMember(d => d.SpecializationName, o => o.MapFrom(s => s.Specialization != null ?
                        s.Specialization.Name : null))
                .ForMember(d => d.HospitalName, o => o.MapFrom(s => s.Hospital != null ? 
                        s.Hospital.Name : null))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? 
                        s.Department.Name : null));

            // CreateUserModel -> User
            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.MustChangePassword, opt => opt.Ignore());

            CreateMap<User, CreateUserResponseModel>();
        }
    }
}
