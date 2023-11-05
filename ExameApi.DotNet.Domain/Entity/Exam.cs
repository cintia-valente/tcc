using System.ComponentModel.DataAnnotations;

namespace ExamApi.DotNet.Domain.Entity;

public class Exam
{
    [Key]
    public Guid IdExam { get; set; }
    public string Name { get; set; }  
    public string Description { get; set; }
    public string UrlLocations { get; set; }
    public Guid? IdPatient { get; set;}
    public Patient? Patient { get; set; }
}
