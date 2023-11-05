
using AutoMapper;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using ExameApi.DotNet.Repository.Interface;

namespace ExameApi.DotNet.Application.Service;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    
    public PatientService(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Patient> Save(Patient patient)
    {
        var patientSaved = await _patientRepository.Save(patient);
        return patientSaved;
    }

    public async Task<IEnumerable<Patient>> FindAll()
    {
        return await _patientRepository.FindAll();
    }

    public async Task<Patient> FindById(Guid id)
    {
        var patientById = await _patientRepository.FindById(id);
        return patientById;
    }
}
