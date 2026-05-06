using AutoMapper;
using MedSchedulerUZ.Application.Models.SpecializationModel;
using MedSchedulerUZ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSchedulerUZ.Application.MappingProfiles
{
    public class SpecializationMapping : Profile
    {
        public SpecializationMapping()
        {
            CreateMap<Specialization, SpecializationResponseModel>();
            CreateMap<CreateSpecializationModel, Specialization>();
        }
    }
}
