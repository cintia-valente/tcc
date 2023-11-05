using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExameApi.DotNet.Application.Profiles;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<PatientDto, Patient>()
             .ForMember(dest => dest.IdPatient, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<Patient, PatientDto>()
          .ForMember(dest => dest.IdPatient, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<EntityEntry<Patient>, Patient>();
    }
}
