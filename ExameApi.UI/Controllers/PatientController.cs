using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ExameApi.UI.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private IPatientService _patientService;
    private IMapper _mapper;

    public PatientController(IPatientService patientService, IMapper mapper)
    {
        _patientService = patientService;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um paciente
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostPatient(
        [FromBody] PatientDto postPatientDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var patientConverter = _mapper.Map<Patient>(postPatientDTO);
        var patientSave = await _patientService.Save(patientConverter);

        return CreatedAtAction(nameof(GetPatientForId), new { id = patientSave.IdPatient }, patientSave);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Patient>> GetCitiesWithWeatherData()
    {
        var result = await _patientService.FindAll();
        return (IEnumerable<Patient>)result;
    }

    /// <summary>
    /// Lista um paciente por id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientForId(Guid id)
    {
        var patient = await _patientService.FindById(id);
        return patient is null ? NotFound("Patient not found") : Ok(patient);
    }
}
