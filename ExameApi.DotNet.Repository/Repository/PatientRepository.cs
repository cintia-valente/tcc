

using AutoMapper;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;
using ExameApi.DotNet.Persistence;
using ExameApi.DotNet.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExameApi.DotNet.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly ExamContext _context;
    private IMapper _mapper;

    public PatientRepository(ExamContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    public async Task<Patient> Save(Patient patient)
    {
        EntityEntry<Patient?> patientEntity = _context.PatientData.Add(patient);
        await _context.SaveChangesAsync();
        var patientConvert = _mapper.Map<Patient>(patientEntity.Entity); 
        return patientConvert;
    }

    public async Task<IEnumerable<Patient>> FindAll()
    {
        return await _context.PatientData.Include(patient => patient.ExamList).ToListAsync();
    }

    public async Task<List<Patient>> GetPatientsByAgeAndGender(int age, Gender gender)
    {
        return await _context.PatientData
            .Where(patient => patient.Age == age && patient.Gender == gender)
            .ToListAsync();
    }

    public async Task<Patient> FindById(Guid? idPatient)
    {
        var x = await _context.PatientData.Include(patient => patient.ExamList).FirstOrDefaultAsync(data => data.IdPatient == idPatient);
        return x;

    }

}
