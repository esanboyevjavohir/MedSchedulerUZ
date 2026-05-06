using AutoMapper;
using MedSchedulerUZ.Application.Models.HospitalModel;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class HospitalMapping : Profile
    {
        public HospitalMapping()
        {
            CreateMap<Hospital, HospitalResponseModel>();
            CreateMap<CreateHospitalModel, Hospital>();
        }
    }
}
