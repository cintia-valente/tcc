
using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using ExameApi.DotNet.Repository.Interface;

namespace ExameApi.DotNet.Application.Service;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
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

    public async Task<Patient> FindById(Guid id, bool tracking = true)
    {
        var patientById = await _patientRepository.FindById(id);

        return patientById;
    }

    public async Task Update(Guid id, PatientDto patientDto)
    {
        var existingPatient = await _patientRepository.FindById(id);

        if (existingPatient == null)
        {
            throw new ArgumentException("Paciente não encontrado.");
        }

        existingPatient.Name = patientDto.Name;
        existingPatient.Age = patientDto.Age;
        existingPatient.Gender = patientDto.Gender;

        await _patientRepository.Update(existingPatient);
    }
}
