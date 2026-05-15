using AutoMapper;
using MedSchedulerUZ.Application.Models.CertificationModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class CertificationMapping : Profile
    {
        public CertificationMapping()
        {
            CreateMap<Certification, CertificationResponseModel>()
                .ForMember(dest => dest.UserFullName,
            opt => opt.MapFrom(src => src.User.FullName));

            CreateMap<AddCertificationModel, Certification>();
        }
    }
}
