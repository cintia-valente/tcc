using ExameApi.DotNet.Domain.Entity.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamApi.DotNet.Domain.Entity;

public class Patient
{
    [Key]
    public Guid IdPatient { get; set; }
    public string Name { get; set; }
    public string Age { get; set; }
    public Gender Gender { get; set; }

    [ForeignKey("IdPatient")]
    public virtual List<Exam> ExamList { get; set; } = new List<Exam>();    
}