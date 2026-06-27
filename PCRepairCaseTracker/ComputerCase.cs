namespace PCRepairCaseTracker.Models;

public class ComputerCase
{
    public int Id { get; set; }

    public string ComputerName { get; set; } = "";

    public string DeviceType { get; set; } = "";

    public string OperatingSystem { get; set; } = "";

    public string MainProblem { get; set; } = "";

    public string Symptoms { get; set; } = "";

    public string WhenItStarted { get; set; } = "";

    public string RecentChanges { get; set; } = "";

    public string TroubleshootingStepsTried { get; set; } = "";

    public string TechnicianNotes { get; set; } = "";

    public SeverityLevel Severity { get; set; }

    public CaseStatus Status { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}