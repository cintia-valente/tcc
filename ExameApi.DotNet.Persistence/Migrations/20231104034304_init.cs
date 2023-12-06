using System;
using System.Text.Json;
using ExamApi.DotNet.Domain.Entity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExameApi.DotNet.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

            migrationBuilder.CreateTable(
                name: "PatientData",
                columns: table => new
                {
                    IdPatient = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientData", x => x.IdPatient);
                });

            migrationBuilder.CreateTable(
                name: "ExamData",
                columns: table => new
                {
                    IdExam = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UrlLocations = table.Column<string>(type: "text", nullable: true),
                    IdPatient = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamData", x => x.IdExam);
                    table.ForeignKey(
                        name: "FK_ExamData_PatientData_IdPatient",
                        column: x => x.IdPatient,
                        principalTable: "PatientData",
                        principalColumn: "IdPatient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamData_IdPatient",
                table: "ExamData",
                column: "IdPatient");

            migrationBuilder.CreateIndex(
                name: "IX_PatientData_Name",
                table: "PatientData",
                column: "Name",
                unique: true);

            var examsData = System.IO.File.ReadAllText("exams.json");
            var exams = JsonSerializer.Deserialize<List<Exam>>(examsData);

            foreach (var exam in exams)
            {
                if (exam.IdExam == Guid.Empty)
                {
                    exam.IdExam = Guid.NewGuid();
                }

                migrationBuilder.InsertData(
                    table: "ExamData",
                    columns: new[] { "IdExam", "Name", "Description", "UrlLocations", "IdPatient" },
                    values: new object[] { exam.IdExam, exam.Name, exam.Description, exam.UrlLocations, exam.IdPatient }
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamData");

            migrationBuilder.DropTable(
                name: "PatientData");
        }
    }
}
