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
                .ForMember(dest => dest.RoleType,
                        opt => opt.MapFrom(src => src.RoleType.ToString()))
                .ForMember(dest => dest.SpecializationName,
                        opt => opt.MapFrom(src => src.Specialization.Name));

            // CreateUserModel -> User
            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.PasswordHash,
                        opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                        opt => opt.MapFrom(src => true));
        }
    }
}
