using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;

namespace ExameApi.DotNet.Repository.Interface;

public interface IExamRepository
{
    Task<Exam> Save(Exam exam);

    Task<List<Exam>> GetExamsByPatientId(Guid patientId);
    Task<List<Exam>> GetExamsByAgeAndGender(int? age, Gender? gender);
    Task<Exam> FindById(Guid idExam, bool tracking = true);

    Task<IQueryable<Exam>> FindAll();
}
