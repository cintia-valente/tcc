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

    [Fact(DisplayName = "Dado um objeto Exam, quando salvar o objeto, ent�o chama os m�todos FindByID e Save exatamente uma vez.")]
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

        var validExam = new Exam(); // Crie um objeto Weather v�lido conforme necess�rio
        var validPatient = new Patient(); // Crie um objeto City v�lido conforme necess�rio

        _patientRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                          .ReturnsAsync(validPatient);

        _examRepositoryMock.Setup(repo => repo.Save(It.IsAny<Exam>()))
                             .ReturnsAsync(validExam); // Configura(Setup) o comportamento que deve ocorrer quando Save for chamado, Save aceita qualquer obj do tipo Weather e retorna um objeto do tipo Weather quando for chamado.

        // Act
        var result = await _examService.Save(validExamDto); //chama o m�todo Save com o mock e armazena o resultado.

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validExam, result);//verifica se o mock passado como par�metro � igual ao obj retornado pelo m�todo Save.

        _patientRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once); //verifica se FindByID foi chamado exatamente uma vez, passando como par�metro qualquer argumento do tipo GUID.
        _examRepositoryMock.Verify(repo => repo.Save(It.IsAny<Exam>()), Times.Once); //verifica se Save foi chamado exatamente uma vez, passando como par�metro o mock criado.
    }

    [Fact(DisplayName = "Dado um objeto Weather, quando passar valores de enums inv�lidos, ent�o lan�a uma exce��o.")]
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

    [Fact(DisplayName = "Dado uma age, gender e uma lista de Exam, quando chamar o m�todo, ent�o verifica se o resultado n�o � null, verifica se o n�mero de exames no resultado � igual ao tamanho da lista e verifica os exames que est�o na lista de retorno.")]
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
            new Exam { Name = "Teste ergom�trico" },
            new Exam { Name = "Densitometria �ssea" },
        };

        _examRepositoryMock.Setup(repo => repo.GetExamsByAgeAndGender(age, gender))
                          .ReturnsAsync(exams);

        // Act
        var result = await _examService.GetExamsByAgeAndGender(age, gender);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Count); // Verifica se o n�mero de exames retornados est� correto
        Assert.Contains("PSA e toque retal", result.Select(exam => exam.Name)); // Verifica se o exame est� na lista de retorno
        Assert.Contains("Teste ergom�trico", result.Select(exam => exam.Name)); // Verifica se o exame est� na lista de retorno
        Assert.Contains("Eletrocardiograma", result.Select(exam => exam.Name)); // Verifica se o exame est� na lista de retorno
    }

    [Fact(DisplayName = "Dado uma chamada ao m�todo GetExamsByAgeAndGender, ent�o lan�a uma exce��o.")]
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

    [Fact(DisplayName = "Dado uma chamada ao m�todo FindAll, ent�o chama o m�todo FindAll exatamente uma vez.")]
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

    [Fact(DisplayName = "Dado uma chamada ao m�todo FindAll, ent�o lan�a uma exce��o.")]
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

    [Fact(DisplayName = "Dado um id do Exam, ent�o chama o m�todo FindById exatamente uma vez.")]
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

    [Fact(DisplayName = "Dado um id do Weather, quando chamar o m�todo FindById, ent�o lan�a uma exce��o.")]
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