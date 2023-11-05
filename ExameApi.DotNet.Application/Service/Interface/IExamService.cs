
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;

namespace ExameApi.DotNet.Application.Service.Interface;

public interface IExamService
{
    Task<Exam> Save(ExamDto examDto);
    // Task<List<Exam>> GetExamsByAgeAndGender(int age, Gender gender);
    Task<List<ExamDto>> GetExamsByAgeAndGender(int? age, Gender? gender);

    Task<IEnumerable<Exam>> FindAll();
    Task<Exam> FindById(Guid id, bool tracking = true);
}
