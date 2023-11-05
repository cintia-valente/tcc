using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;

namespace ExameApi.DotNet.Repository.Interface;

public interface IPatientRepository
{
    Task<Patient> Save(Patient patient);
    Task<IEnumerable<Patient>> FindAll();

    Task<List<Patient>> GetPatientsByAgeAndGender(int age, Gender gender);
    Task<Patient?> FindById(Guid? idPatient);
}
