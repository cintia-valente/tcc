using Moq;
using ExameApi.DotNet.Repository.Interface;
using ExameApi.DotNet.Application.Service;
using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Domain.Entity.Enum;
using System.Collections.Generic;

namespace ExamApi.Test;

public class ExamServiceTest
{
    private readonly Mock<IExamRepository> _examRepositoryMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ExamService _examService;

    public ExamServiceTest()
    {
        _examRepositoryMock = new Mock<IExamRepository>();
        _patientRepositoryMock = new Mock<IPatientRepository>();

        var configuration = new MapperConfiguration(cfg => {
            cfg.CreateMap<ExamDto, Exam>();
            cfg.CreateMap<Exam, ExamDto>();
        });

        _mapper = configuration.CreateMapper();

        _examService = new ExamService(_examRepositoryMock.Object, _patientRepositoryMock.Object, _mapper);
    }

    [Fact(DisplayName = "Dado um objeto Exam, quando salvar o objeto, então chama os métodos FindByID e Save exatamente uma vez.")]
    public async Task SaveSucess()
    {
        // Arrange
        var validPatienDto = new Patient
        {
            IdPatient = Guid.NewGuid(),
            Name = "Mia",
            Age = 16,
            Gender = Gender.F
        };

        var validExamDto = new ExamDto
        {
            IdExam = Guid.NewGuid(),
            Name = "Glicemia",
            Description = "Teste",
            UrlLocations = "www.google.com",
            IdPatient = Guid.NewGuid(),
            Patient = validPatienDto
        };

        var validExam = new Exam(); // Crie um objeto Weather válido conforme necessário
        var validPatient = new Patient(); // Crie um objeto City válido conforme necessário

        _patientRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                          .ReturnsAsync(validPatient);

        _examRepositoryMock.Setup(repo => repo.Save(It.IsAny<Exam>()))
                             .ReturnsAsync(validExam); // Configura(Setup) o comportamento que deve ocorrer quando Save for chamado, Save aceita qualquer obj do tipo Weather e retorna um objeto do tipo Weather quando for chamado.

        // Act
        var result = await _examService.Save(validExamDto); //chama o método Save com o mock e armazena o resultado.

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validExam, result);//verifica se o mock passado como parâmetro é igual ao obj retornado pelo método Save.

        _patientRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once); //verifica se FindByID foi chamado exatamente uma vez, passando como parâmetro qualquer argumento do tipo GUID.
        _examRepositoryMock.Verify(repo => repo.Save(It.IsAny<Exam>()), Times.Once); //verifica se Save foi chamado exatamente uma vez, passando como parâmetro o mock criado.
    }

    [Fact(DisplayName = "Dado um objeto Weather, quando passar valores de enums inválidos, então lança uma exceção.")]
    public async Task SaveWeatherEnumsError()
    {
        // Arrange
        var invalidPatienDto = new Patient
        {
            IdPatient = Guid.NewGuid(),
            Name = "",
            Age = 16,
            Gender = Gender.F
        };

        var invalidExamDto = new ExamDto
        {
            IdExam = Guid.NewGuid(),
            Name = "Glicemia",
            Description = "Teste",
            UrlLocations = "www.google.com",
            IdPatient = Guid.NewGuid(),
            Patient = invalidPatienDto
        };

        _examRepositoryMock.Setup(repo => repo.Save(It.IsAny<Exam>()))
        .Throws<Exception>();

        await Assert.ThrowsAsync<Exception>(async () => await _examService.Save(invalidExamDto));
    }

    [Fact(DisplayName = "Dado uma age, gender e uma lista de Exam, quando chamar o método, então verifica se o resultado não é null, verifica se o número de exames no resultado é igual ao tamanho da lista e verifica os exames que estão na lista de retorno.")]
    public async Task GetExamsByAgeAndGender_ReturnsFilteredExams()
    {
        // Arrange
        int age = 40; 
        Gender gender = Gender.M; 

        var exams = new List<Exam>
        {
            new Exam { Name = "Hemograma completo" },
            new Exam { Name = "Exame de urina" },
            new Exam { Name = "Glicemia" },
            new Exam { Name = "Eletrocardiograma" },
            new Exam { Name = "PSA e toque retal" },
            new Exam { Name = "Teste ergométrico" },
            new Exam { Name = "Densitometria óssea" },
        };

        _examRepositoryMock.Setup(repo => repo.GetExamsByAgeAndGender(age, gender))
                          .ReturnsAsync(exams);

        // Act
        var result = await _examService.GetExamsByAgeAndGender(age, gender);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Count); // Verifica se o número de exames retornados está correto
        Assert.Contains("PSA e toque retal", result.Select(exam => exam.Name)); // Verifica se o exame está na lista de retorno
        Assert.Contains("Teste ergométrico", result.Select(exam => exam.Name)); // Verifica se o exame está na lista de retorno
        Assert.Contains("Eletrocardiograma", result.Select(exam => exam.Name)); // Verifica se o exame está na lista de retorno
    }

    [Fact(DisplayName = "Dado uma chamada ao método GetExamsByAgeAndGender, então lança uma exceção.")]
    public async Task GetExamsByAgeAndGenderError()
    {
        // Arrange
        int age = 40;
        Gender gender = Gender.M;

        _examRepositoryMock.Setup(repo => repo.GetExamsByAgeAndGender(age, gender))
                          .ThrowsAsync(new Exception("Simulated error"));

        // Assert
        await Assert.ThrowsAsync<Exception>(() => _examService.GetExamsByAgeAndGender(age, gender));
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então chama o método FindAll exatamente uma vez.")]
    public async Task FindAllSucess()
    {
        // Arrange
        var examList = new List<Exam>
        {
             new Exam
            {
                 IdExam = Guid.NewGuid(),
                Name = "Glicemia",
                Description = "Teste",
                UrlLocations = "www.google.com",
                IdPatient = Guid.NewGuid(),
                Patient = new Patient
                {
                    IdPatient = Guid.NewGuid(),
                    Name = "Mia",
                    Age = 16,
                    Gender = Gender.F
                }
            }
        };

        _examRepositoryMock.Setup(repo => repo.FindAll())
        .ReturnsAsync(examList.AsQueryable());

        // Act
        var result = await _examService.FindAll();

        // Assert
        Assert.Equal(examList, result);
        _examRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então lança uma exceção.")]
    public void FindAllError()
    {
        // Arrange
        _examRepositoryMock.Setup(repo => repo.FindAll())
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _examService.FindAll());

        // Assert
        _ = Assert.ThrowsAsync<Exception>(() => _examService.FindAll());
    }

    [Fact(DisplayName = "Dado um id do Exam, então chama o método FindById exatamente uma vez.")]
    public async Task FindByIdSucess()
    {
        // Arrange
        Guid idExam = Guid.NewGuid();
        bool tracking = true;

        _examRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>(), It.IsAny<bool>()))
        .ReturnsAsync((Guid id, bool tracking) => new Exam { IdExam = id });

        // Act
        var result = await _examService.FindById(idExam, tracking);

        // Assert
        Assert.Equal(idExam, result.IdExam);
        _examRepositoryMock.Verify(repo => repo.FindById(idExam, tracking), Times.Once);
    }

    [Fact(DisplayName = "Dado um id do Weather, quando chamar o método FindById, então lança uma exceção.")]
    public void FindByIdError()
    {
        // Arrange
        Guid idExam = Guid.NewGuid();
        bool tracking = true;

        _examRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>(), It.IsAny<bool>()))
        .Throws<Exception>();

        // Act
        var exceptionFindById = Assert.ThrowsAsync<Exception>(() => _examService.FindById(idExam, tracking));

        // Assert
        _ = Assert.ThrowsAsync<Exception>(() => _examService.FindById(idExam, tracking));
    }
}