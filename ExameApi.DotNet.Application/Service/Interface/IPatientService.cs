
using ExamApi.DotNet.Domain.Entity;
using System.Threading.Tasks;

namespace ExameApi.DotNet.Application.Service.Interface;

public interface IPatientService
{
    Task<Patient> Save(Patient patient);
    Task<IEnumerable<Patient>> FindAll();
    Task<Patient> FindById(Guid id);
}
