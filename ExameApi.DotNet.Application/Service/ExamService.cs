using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using ExameApi.DotNet.Domain.Entity.Enum;
using ExameApi.DotNet.Repository.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace ExameApi.DotNet.Application.Service;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public ExamService(IExamRepository examRepository, IPatientRepository patientRepository, IMapper mapper)
    {
        _examRepository = examRepository;
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<Exam> Save(ExamDto examDto)
    {
        var examConverter = _mapper.Map<Exam>(examDto);

        examConverter.Patient = await _patientRepository.FindById(examDto.IdPatient);

        var weatherSaved = await _examRepository.Save(examConverter);

        return weatherSaved;
    }
    
    public async Task<List<ExamDto>> GetExamsByAgeAndGender(int? age, Gender? gender)
    {
        var exams = await _examRepository.GetExamsByAgeAndGender(age, gender);

        List<string> requiredExams = new List<string>();

        if (age >= 0)
        {
            requiredExams.AddRange(new List<string> { "Hemograma completo", "Exame de urina", "Glicemia", "Eletrocardiograma" });
        }

        if (gender == Gender.F && age >= 25 && age <= 64)
        {
            requiredExams.Add("Papanicolau");
        }

        if (gender == Gender.F && age >= 40)
        {
            requiredExams.Add("Mamografia");
        }

        if (gender == Gender.M && age >= 40)
        {
            requiredExams.AddRange(new List<string> { "PSA e toque retal" });
        }

        if (age <= 12)
        {
            requiredExams.AddRange(new List<string> { "Parasitológico (fezes)", "Glicemia e insulina", "Sorologia" });
        }

        if (age <= 18)
        {
            requiredExams.AddRange(new List<string> {"Lipidograma", "PCR" });
        }

        if (age >= 35)
        {
            requiredExams.Add("TSH e T4");
        }

        if (age >= 40)
        {
            requiredExams.Add("Teste ergométrico");
        }

        if (age >= 50)
        {
            requiredExams.Add("Densitometria óssea");
        }

        var filteredExams = exams.Where(exam => requiredExams.Contains(exam.Name)).ToList();

        return _mapper.Map<List<ExamDto>>(filteredExams);
    }

    public async Task<IEnumerable<Exam>> FindAll()
    {
        var examsData = await _examRepository.FindAll();

        return examsData;
    }

    public async Task<Exam> FindById(Guid id, bool tracking = true)
    {
        var examData = await _examRepository.FindById(id, tracking);
        return examData;
    }

}
