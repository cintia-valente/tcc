using ExamApi.DotNet.Domain.Data.Dtos;
using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Application.Service.Interface;
using ExameApi.DotNet.Domain.Entity.Enum;
using Microsoft.AspNetCore.Mvc;

namespace ExameApi.UI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamController : ControllerBase
{
    private IExamService _examService;

    public ExamController(IExamService examService)
    {
        _examService = examService;
    }

    /// <summary>
    /// Cria um exame
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostExam(
        [FromBody] ExamDto postExamDTO)
    {
        var examSave = await _examService.Save(postExamDTO);
        return CreatedAtAction(nameof(GetExamById), new { id = examSave.IdExam }, examSave);
    }

    /// <summary>
    /// Lista os exames pelo sexo e idade do paciente.
    /// </summary>
    [HttpGet("list-exams/{age}/{gender}")]
    public async Task<IActionResult> GetExamsByAgeAndGender([FromRoute] int? age, [FromRoute] Gender? gender)
    {
        var exams = await _examService.GetExamsByAgeAndGender(age, gender);
        return Ok(exams);
    }

    /// <summary>
    /// Lista todos os exames
    /// </summary>
    [HttpGet()]
    public async Task<IEnumerable<Exam>> GetWeatherWithWeatherData()
    {
        var weatherAll = await _examService.FindAll();

        return weatherAll.ToList();
    }

    /// <summary>
    /// Lista registros de exames pelo id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExamById(Guid id)
    {
        var weather = await _examService.FindById(id);

        return Ok(weather);
    }

}