using System.Text.Json;
using PCRepairCaseTracker.Models;

namespace PCRepairCaseTracker.Services;

public class FileStorage
{
    private readonly string _filePath = "pc_cases.json";

    public List<ComputerCase> LoadCases()
    {
        if (!File.Exists(_filePath))
        {
            return new List<ComputerCase>();
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<ComputerCase>();
        }

        return JsonSerializer.Deserialize<List<ComputerCase>>(json) ?? new List<ComputerCase>();
    }

    public void SaveCases(List<ComputerCase> cases)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(cases, options);

        File.WriteAllText(_filePath, json);
    }
}