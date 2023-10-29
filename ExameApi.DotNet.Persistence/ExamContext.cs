using ExamApi.DotNet.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExameApi.DotNet.Persistence;

public class ExamContext : DbContext
{
    public ExamContext(DbContextOptions<ExamContext> options)
        : base(options)
    {
    }

    public DbSet<Exam> ExamData { get; set; }
    public DbSet<Patient> PatientData { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.Name)
            .IsUnique();

        modelBuilder.Entity<Patient>()
            .Property(p => p.IdPatient)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Exam>()
            .Property(e => e.IdExam)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Patient>()
            .HasMany(patient => patient.ExamList)
            .WithOne(exam => exam.Patient)
            .HasForeignKey(exam => exam.IdPatient)
            .OnDelete(DeleteBehavior.Cascade);
    }
}