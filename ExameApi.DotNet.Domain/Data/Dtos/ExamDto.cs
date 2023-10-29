using ExamApi.DotNet.Domain.Entity;

namespace ExamApi.DotNet.Domain.Data.Dtos;

public class ExamDto
{
    public Guid IdExam { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string UrlLocations { get; set; }

    public Guid IdPatient { get; set; }

    public Patient? Patient { get; set; }

}
