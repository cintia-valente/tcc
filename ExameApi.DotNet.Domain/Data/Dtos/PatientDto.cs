using ExameApi.DotNet.Domain.Entity.Enum;

namespace ExamApi.DotNet.Domain.Data.Dtos;

public class PatientDto
{
    public Guid IdPatient { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}
