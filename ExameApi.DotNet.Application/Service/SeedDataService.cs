using ExamApi.DotNet.Domain.Entity;
using ExameApi.DotNet.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExameApi.DotNet.Application.Service;

public class SeedDataService
{
    private readonly ExamContext _context;

    public SeedDataService(ExamContext context)
    {
        _context = context;
    }

    public void SeedExamsFromJson(string jsonFilePath)
    {
        if (!_context.ExamData.Any())
        {
            var json = File.ReadAllText(jsonFilePath);
            var exams = JsonConvert.DeserializeObject<List<Exam>>(json);

            _context.ExamData.AddRange(exams);
            _context.SaveChanges();
        }
    }
}
