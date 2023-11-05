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

public class ExamProfile : Profile
{
    public ExamProfile()
    {
        CreateMap<ExamDto, Exam>()
            .ForMember(dest => dest.IdExam, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<Exam, ExamDto>()
           .ForMember(dest => dest.IdExam, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<EntityEntry<Exam>, Exam>();
    }
}
