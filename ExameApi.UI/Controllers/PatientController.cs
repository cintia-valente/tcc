using AutoMapper;
using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    /// Cria um paciente.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostPatient(
        [FromBody] PatientDto postPatientDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var patientSave = await _patientService.Save(postPatientDTO);

        return CreatedAtAction(nameof(GetPatientForId), new { id = patientSave.IdPatient }, patientSave);
    }

    /// <summary>
    /// Lista todos os pacientes.
    /// </summary>
    [HttpGet("all")]
    public async Task<IEnumerable<Patient>> GetPatient()
    {
        var result = await _patientService.FindAll();
        return result.ToList();
    }

    /// <summary>
    /// Lista um paciente por id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientForId(Guid idPatient)
    {
        var patient = await _patientService.FindById(idPatient);
        return Ok(patient);
    }

    /// <summary>
    /// Atualiza um paciente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(Guid id, [FromBody] PatientDto patientDto)
    {
        try
        {
            await _patientService.Update(id, patientDto);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
