using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service;
using ExameApi.DotNet.Domain.Entity.Enum;
using ExameApi.DotNet.Repository.Interface;
using Moq;

namespace ExamApi.Test;

public class PatientServiceTest
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly IMapper _mapper;
    private readonly PatientService _patientService;

    public PatientServiceTest()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();

        var configuration = new MapperConfiguration(cfg => {
            cfg.CreateMap<PatientDto, Patient>();
            cfg.CreateMap<Patient, PatientDto>();
        });

        _mapper = configuration.CreateMapper();

        _patientService = new PatientService(_patientRepositoryMock.Object, _mapper);
    }

    [Fact(DisplayName = "Dado um objeto Patient, quando salvar o objeto, então chama os métodos FindByID e Save exatamente uma vez.")]
    public async Task SaveSucess()
    {
        // Arrange
        var validPatienDto = new PatientDto
        {
            IdPatient = Guid.NewGuid(),
            Name = "Mia",
            Age = 16,
            Gender = Gender.F
        };

        var validPatient = new Patient();

        _patientRepositoryMock.Setup(repo => repo.Save(It.Is<Patient>(p => p.IdPatient == validPatienDto.IdPatient && p.Name == validPatienDto.Name)))
                 .ReturnsAsync(validPatient);

        // Act
        var result = await _patientService.Save(validPatienDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validPatient, result);

        _patientRepositoryMock.Verify(repo => repo.Save(It.IsAny<Patient>()), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada de Save, então lança uma exceção.")]
    public async Task SaveError()
    {
        // Arrange
        var invalidPatienDto = new PatientDto
        {
            IdPatient = Guid.NewGuid(),
            Name = "",
            Age = 16,
            Gender = Gender.F
        };

        _patientRepositoryMock.Setup(repo => repo.Save(It.IsAny<Patient>()))
        .Throws<ArgumentException>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<ArgumentException>(() => _patientService.Save(invalidPatienDto));

        // Assert
        Assert.ThrowsAsync<ArgumentException>(() => _patientService.Save(invalidPatienDto));
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então chama o método FindAll exatamente uma vez.")]
    public async Task FindAllSucess()
    {
        // Arrange
        var cityList = new List<Patient>
        {
              new Patient
              {
                  IdPatient = Guid.NewGuid(),
                  Name = "Mia",
                  Age = 16,
                  Gender = Gender.F
              }
        };

        _patientRepositoryMock.Setup(repo => repo.FindAll())
        .ReturnsAsync(cityList.AsQueryable());

        // Act
        var result = await _patientService.FindAll();

        // Assert
        Assert.Equal(cityList, result);
        _patientRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então lança uma exceção.")]
    public async Task FindAllError()
    {
        // Arrange
        _patientRepositoryMock.Setup(repo => repo.FindAll())
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _patientService.FindAll());

        // Assert
        Assert.ThrowsAsync<Exception>(() => _patientService.FindAll());
    }

    [Fact(DisplayName = "Dado um id de Patient, então chama o método FindById exatamente uma vez.")]
    public async Task FindByIdSucess()
    {
        // Arrange
        Guid idPatient = Guid.NewGuid();

        _patientRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .ReturnsAsync((Guid id) => new Patient { IdPatient = id });

        // Act
        var result = await _patientService.FindById(idPatient);

        // Assert
        Assert.Equal(idPatient, result.IdPatient);
        _patientRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Dado um id de Patient, quando chamar o método FindById, então lança uma exceção.")]
    public async Task FindByIdError()
    {
        // Arrange
        Guid idCity = Guid.NewGuid();

        _patientRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .Throws<Exception>();

        // Act
        var exceptionFindById = Assert.ThrowsAsync<Exception>(() => _patientService.FindById(idCity));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _patientService.FindById(idCity));
    }

    [Fact(DisplayName = "Dado um id de Patient, quando chamar o método Update, então verifica se os dados foram atualizados e chama o método Update exatamente uma vez.")]
    public async Task UpdateSuccess()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var existingPatient = new Patient
        {
            IdPatient = patientId,
            Name = "Manoel",
            Age = 27,
            Gender = Gender.F
        };

        var patientDto = new PatientDto
        {
            IdPatient = patientId,
            Name = "Miguel",
            Age = 23,
            Gender = Gender.M
        };

        _patientRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>())).ReturnsAsync(existingPatient);

        // Act
        await _patientService.Update(patientId, patientDto);

        // Assert
        Assert.Equal("Miguel", existingPatient.Name);
        Assert.Equal(23, existingPatient.Age);
        Assert.Equal(Gender.M, existingPatient.Gender);
        _patientRepositoryMock.Verify(repo => repo.Update(existingPatient), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada de Update, então lança uma exceção.")]
    public async Task UpdateError()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var existingPatient = new Patient
        {
            IdPatient = patientId,
            Name = "",
            Age = 27,
            Gender = Gender.F
        };

        var invalidPatienDto = new PatientDto
        {
            IdPatient = patientId,
            Name = "Miguel",
            Age = 23,
            Gender = Gender.M
        };

        _patientRepositoryMock.Setup(repo => repo.Save(It.IsAny<Patient>()))
        .Throws<ArgumentException>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<ArgumentException>(() => _patientService.Save(invalidPatienDto));

        // Assert
        Assert.ThrowsAsync<ArgumentException>(() => _patientService.Save(invalidPatienDto));
    }

}
