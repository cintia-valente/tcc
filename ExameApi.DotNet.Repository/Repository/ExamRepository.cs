using AutoMapper;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;
using ExameApi.DotNet.Persistence;
using ExameApi.DotNet.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExameApi.DotNet.Repository.Repository;

public class ExamRepository : IExamRepository
{
    private readonly ExamContext _context;
    private readonly IMapper _mapper;

    public ExamRepository(ExamContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Exam> Save(Exam exam)
    {
        EntityEntry<Exam> examEntity = _context.ExamData.Add(exam);

        await _context.SaveChangesAsync();
        var examConvert = _mapper.Map<Exam>(examEntity.Entity);

        return examConvert;
    }

    public async Task<List<Exam>> GetExamsByAgeAndGender(int? age, Gender? gender)
    {
        var examsQuery = _context.ExamData.AsQueryable();

        if (age.HasValue && gender.HasValue)
        {
            examsQuery = examsQuery.Where(exam =>
                (exam.Patient != null && exam.Patient.Age == age && exam.Patient.Gender == gender) ||
                exam.Patient == null
            );
        }

        var exams = await examsQuery.Include(exam => exam.Patient).AsNoTracking().ToListAsync();
        return exams;
    }

    public async Task<IQueryable<Exam>> FindAll()
    {
        var examsData = _context.ExamData
            .Include(e => e.Patient)
            .Select(x => new Exam
            {
                Patient = x.Patient != null
                    ? new Patient
                    {
                        IdPatient = x.Patient.IdPatient,
                        Name = x.Patient.Name,
                        Age = x.Patient.Age,
                        Gender = x.Patient.Gender,
                        ExamList = x.Patient.ExamList
                    }
                    : null,
                IdExam = x.IdExam,
                Name = x.Name,
                Description = x.Description,
                UrlLocations = x.UrlLocations
            });

        return examsData.AsQueryable();
    }

    public async Task<List<Exam>> GetExamsByPatientId(Guid patientId)
    {
        return await _context.ExamData
            .Where(exam => exam.IdPatient == patientId)
            .ToListAsync();
    }

    public async Task<Exam> FindById(Guid idExam, bool tracking = true)
    {
        if (tracking)
        {
            return await _context.ExamData.Include(e => e.Patient).FirstOrDefaultAsync(metData => metData.IdExam == idExam);
        }

        return await _context.ExamData.Include(e => e.Patient).AsNoTracking().FirstOrDefaultAsync(metData => metData.IdExam == idExam);
    }

}
