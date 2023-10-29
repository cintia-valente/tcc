using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExameApi.DotNet.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientData",
                columns: table => new
                {
                    IdPatient = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<string>(type: "text", nullable: false),
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
                    IdExam = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UrlLocations = table.Column<string>(type: "text", nullable: false),
                    IdPatient = table.Column<Guid>(type: "uuid", nullable: false)
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
