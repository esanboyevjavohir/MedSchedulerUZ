using AutoMapper;
using MedSchedulerUZ.Application.Models.ResponseModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleResponseModel>();
            CreateMap<CreateRoleModel, Role>();
        }
    }
}
