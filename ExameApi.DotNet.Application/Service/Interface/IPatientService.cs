using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;

namespace ExameApi.DotNet.Application.Service.Interface;

public interface IPatientService
{
    Task<Patient> Save(PatientDto patientDto);
    Task<IEnumerable<Patient>> FindAll();
    Task<Patient> FindById(Guid id, bool tracking = true);
    Task Update(Guid idPatient, PatientDto patientDto);
}
