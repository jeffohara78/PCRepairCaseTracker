using PCRepairCaseTracker.Models;

namespace PCRepairCaseTracker.Services;

public class CaseManager
{
    private readonly FileStorage _storage = new();
    private readonly List<ComputerCase> _cases;

    public CaseManager()
    {
        _cases = _storage.LoadCases();
    }

    public List<ComputerCase> GetAllCases()
    {
        return _cases;
    }

    public void AddCase(ComputerCase computerCase)
    {
        computerCase.Id = _cases.Count == 0 ? 1 : _cases.Max(c => c.Id) + 1;
        computerCase.CreatedDate = DateTime.Now;

        _cases.Add(computerCase);
        _storage.SaveCases(_cases);
    }

    public ComputerCase? GetCaseById(int id)
    {
        return _cases.FirstOrDefault(c => c.Id == id);
    }

    public bool UpdateStatus(int id, CaseStatus status)
    {
        ComputerCase? computerCase = GetCaseById(id);

        if (computerCase == null)
        {
            return false;
        }

        computerCase.Status = status;
        _storage.SaveCases(_cases);

        return true;
    }

    public bool AddTechnicianNotes(int id, string notes)
    {
        ComputerCase? computerCase = GetCaseById(id);

        if (computerCase == null)
        {
            return false;
        }

        computerCase.TechnicianNotes = notes;
        _storage.SaveCases(_cases);

        return true;
    }

    public bool DeleteCase(int id)
    {
        ComputerCase? computerCase = GetCaseById(id);

        if (computerCase == null)
        {
            return false;
        }

        _cases.Remove(computerCase);
        _storage.SaveCases(_cases);

        return true;
    }

    public List<ComputerCase> GetOpenCases()
    {
        return _cases
            .Where(c => c.Status != CaseStatus.Resolved)
            .ToList();
    }

    public List<ComputerCase> GetCriticalCases()
    {
        return _cases
            .Where(c => c.Severity == SeverityLevel.Critical)
            .ToList();
    }
}