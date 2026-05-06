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
                .ForMember(dest => dest.RoleName,
                           opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.SpecializationName,
                           opt => opt.MapFrom(src => src.Specialization.Name));

            // CreateUserModel -> User
            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.PasswordHash,
                           opt => opt.Ignore()) // hashni serviceda qilamiz
                .ForMember(dest => dest.EmployeeCode,
                           opt => opt.Ignore()) // serviceda generatsiya qilamiz
                .ForMember(dest => dest.IsActive,
                           opt => opt.MapFrom(src => true));
        }
    }
}
