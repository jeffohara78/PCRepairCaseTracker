using PCRepairCaseTracker.Models;
using PCRepairCaseTracker.Services;

namespace PCRepairCaseTracker.UI;

public class ConsoleUI
{
    private readonly CaseManager _caseManager = new();

    public void Run()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            ShowHeader();

            Console.WriteLine("1. Add new PC repair case");
            Console.WriteLine("2. View all cases");
            Console.WriteLine("3. View technician summary");
            Console.WriteLine("4. Update case status");
            Console.WriteLine("5. Add technician notes");
            Console.WriteLine("6. View dashboard");
            Console.WriteLine("7. Delete case");
            Console.WriteLine("8. Exit");

            Console.Write("\nChoose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCase();
                    break;
                case "2":
                    ViewAllCases();
                    break;
                case "3":
                    ViewTechnicianSummary();
                    break;
                case "4":
                    UpdateStatus();
                    break;
                case "5":
                    AddTechnicianNotes();
                    break;
                case "6":
                    ViewDashboard();
                    break;
                case "7":
                    DeleteCase();
                    break;
                case "8":
                    running = false;
                    break;
                default:
                    Pause("Invalid choice.");
                    break;
            }
        }
    }

    private void ShowHeader()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("        PC REPAIR CASE TRACKER");
        Console.WriteLine("=======================================\n");
    }

    private void AddCase()
    {
        Console.Clear();
        ShowHeader();

        Console.WriteLine("Add New PC Repair Case");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("This form helps document what is happening with a computer before repair.");
        Console.WriteLine("Enter 0 at any time to cancel and return to the main menu.\n");

        ComputerCase computerCase = new();

        Console.WriteLine("COMPUTER NAME");
        Console.WriteLine("Example: Office Desktop, Gaming PC, Dad's Laptop, Dell XPS");
        string? computerName = ReadInputOrCancel("Computer name: ");
        if (computerName == null) return;
        computerCase.ComputerName = computerName;

        Console.WriteLine();

        Console.WriteLine("DEVICE TYPE");
        Console.WriteLine("Examples: Desktop, Laptop, All-in-One, Custom Build");
        string? deviceType = ReadInputOrCancel("Device type: ");
        if (deviceType == null) return;
        computerCase.DeviceType = deviceType;

        Console.WriteLine();

        Console.WriteLine("OPERATING SYSTEM");
        Console.WriteLine("Examples: Windows 10, Windows 11, Linux Mint, Unknown");
        string? os = ReadInputOrCancel("Operating system: ");
        if (os == null) return;
        computerCase.OperatingSystem = os;

        Console.WriteLine();

        Console.WriteLine("MAIN PROBLEM");
        Console.WriteLine("Briefly describe the main issue.");
        Console.WriteLine("Examples:");
        Console.WriteLine("- Windows will not load");
        Console.WriteLine("- PC turns on but shows a black screen");
        Console.WriteLine("- Computer randomly shuts down");
        Console.WriteLine("- Blue screen error appears");
        string? mainProblem = ReadInputOrCancel("Main problem: ");
        if (mainProblem == null) return;
        computerCase.MainProblem = mainProblem;

        Console.WriteLine();

        Console.WriteLine("SYMPTOMS");
        Console.WriteLine("Describe what you see, hear, or notice.");
        Console.WriteLine("Examples:");
        Console.WriteLine("- Fans spin but no display");
        Console.WriteLine("- Stuck on Windows loading screen");
        Console.WriteLine("- Clicking noise from computer");
        Console.WriteLine("- Error message appears before login");
        string? symptoms = ReadInputOrCancel("Symptoms: ");
        if (symptoms == null) return;
        computerCase.Symptoms = symptoms;

        Console.WriteLine();

        Console.WriteLine("WHEN IT STARTED");
        Console.WriteLine("Examples: Today, 3 days ago, after a power outage, after Windows update");
        string? whenStarted = ReadInputOrCancel("When did it start? ");
        if (whenStarted == null) return;
        computerCase.WhenItStarted = whenStarted;

        Console.WriteLine();

        Console.WriteLine("RECENT CHANGES");
        Console.WriteLine("List anything that changed before the problem started.");
        Console.WriteLine("Examples:");
        Console.WriteLine("- Installed Windows update");
        Console.WriteLine("- Added new RAM");
        Console.WriteLine("- Power outage happened");
        Console.WriteLine("- Installed new software");
        Console.WriteLine("- No known changes");
        string? recentChanges = ReadInputOrCancel("Recent changes: ");
        if (recentChanges == null) return;
        computerCase.RecentChanges = recentChanges;

        Console.WriteLine();

        Console.WriteLine("TROUBLESHOOTING STEPS TRIED");
        Console.WriteLine("List what has already been attempted.");
        Console.WriteLine("Examples:");
        Console.WriteLine("- Restarted computer");
        Console.WriteLine("- Checked power cable");
        Console.WriteLine("- Tried different monitor");
        Console.WriteLine("- Booted into recovery mode");
        Console.WriteLine("- Nothing tried yet");
        string? steps = ReadInputOrCancel("Steps tried: ");
        if (steps == null) return;
        computerCase.TroubleshootingStepsTried = steps;

        Console.WriteLine();

        computerCase.Severity = ChooseSeverity();
        computerCase.Status = CaseStatus.New;

        _caseManager.AddCase(computerCase);

        Pause("PC repair case added successfully.");
    }

    private void ViewAllCases()
    {
        Console.Clear();
        ShowHeader();

        List<ComputerCase> cases = _caseManager.GetAllCases();

        if (!cases.Any())
        {
            Pause("No cases have been added yet.");
            return;
        }

        foreach (ComputerCase computerCase in cases)
        {
            DisplayCase(computerCase);
        }

        Pause();
    }

    private void ViewTechnicianSummary()
    {
        Console.Clear();
        ShowHeader();

        DisplayCaseSummary();

        int id = ReadInt("\nEnter case ID to view technician summary: ");

        ComputerCase? computerCase = _caseManager.GetCaseById(id);

        if (computerCase == null)
        {
            Pause("Case not found.");
            return;
        }

        Console.Clear();
        ShowHeader();

        Console.WriteLine("Technician Summary");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Case ID: {computerCase.Id}");
        Console.WriteLine($"Computer: {computerCase.ComputerName}");
        Console.WriteLine($"Device Type: {computerCase.DeviceType}");
        Console.WriteLine($"Operating System: {computerCase.OperatingSystem}");
        Console.WriteLine($"Severity: {computerCase.Severity}");
        Console.WriteLine($"Status: {computerCase.Status}");
        Console.WriteLine($"Created: {computerCase.CreatedDate.ToShortDateString()}");

        Console.WriteLine("\nProblem:");
        Console.WriteLine(computerCase.MainProblem);

        Console.WriteLine("\nSymptoms:");
        Console.WriteLine(computerCase.Symptoms);

        Console.WriteLine("\nWhen It Started:");
        Console.WriteLine(computerCase.WhenItStarted);

        Console.WriteLine("\nRecent Changes:");
        Console.WriteLine(computerCase.RecentChanges);

        Console.WriteLine("\nTroubleshooting Already Tried:");
        Console.WriteLine(computerCase.TroubleshootingStepsTried);

        Console.WriteLine("\nTechnician Notes:");
        Console.WriteLine(string.IsNullOrWhiteSpace(computerCase.TechnicianNotes)
            ? "No technician notes added yet."
            : computerCase.TechnicianNotes);

        Pause();
    }

    private void UpdateStatus()
    {
        Console.Clear();
        ShowHeader();

        DisplayCaseSummary();

        int id = ReadInt("\nEnter case ID to update status: ");

        Console.WriteLine("\nChoose new status:");
        Console.WriteLine("1. New");
        Console.WriteLine("2. In Progress");
        Console.WriteLine("3. Waiting For Repair Shop");
        Console.WriteLine("4. Monitoring");
        Console.WriteLine("5. Resolved");

        int choice = ReadInt("\nStatus choice: ");

        if (choice < 1 || choice > 5)
        {
            Pause("Invalid status.");
            return;
        }

        bool updated = _caseManager.UpdateStatus(id, (CaseStatus)choice);

        Pause(updated ? "Status updated successfully." : "Case not found.");
    }

    private void AddTechnicianNotes()
    {
        Console.Clear();
        ShowHeader();

        DisplayCaseSummary();

        int id = ReadInt("\nEnter case ID to add technician notes: ");

        Console.WriteLine("\nExamples:");
        Console.WriteLine("- Suspect failing SSD");
        Console.WriteLine("- Recommend hardware diagnostics");
        Console.WriteLine("- Possible corrupted Windows installation");
        Console.WriteLine("- User should back up data before repair attempt");

        Console.Write("\nTechnician notes: ");
        string notes = Console.ReadLine() ?? "";

        bool updated = _caseManager.AddTechnicianNotes(id, notes);

        Pause(updated ? "Technician notes saved." : "Case not found.");
    }

    private void ViewDashboard()
    {
        Console.Clear();
        ShowHeader();

        List<ComputerCase> cases = _caseManager.GetAllCases();

        if (!cases.Any())
        {
            Pause("No cases have been added yet.");
            return;
        }

        int total = cases.Count;
        int open = _caseManager.GetOpenCases().Count;
        int critical = _caseManager.GetCriticalCases().Count;
        int resolved = cases.Count(c => c.Status == CaseStatus.Resolved);

        Console.WriteLine("Repair Case Dashboard");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Total Cases: {total}");
        Console.WriteLine($"Open Cases: {open}");
        Console.WriteLine($"Critical Cases: {critical}");
        Console.WriteLine($"Resolved Cases: {resolved}");

        Console.WriteLine("\nCases by Status:");
        foreach (CaseStatus status in Enum.GetValues(typeof(CaseStatus)))
        {
            int count = cases.Count(c => c.Status == status);
            Console.WriteLine($"- {status}: {count}");
        }

        Console.WriteLine("\nCases by Severity:");
        foreach (SeverityLevel severity in Enum.GetValues(typeof(SeverityLevel)))
        {
            int count = cases.Count(c => c.Severity == severity);
            Console.WriteLine($"- {severity}: {count}");
        }

        Pause();
    }

    private void DeleteCase()
    {
        Console.Clear();
        ShowHeader();

        DisplayCaseSummary();

        int id = ReadInt("\nEnter case ID to delete: ");

        bool deleted = _caseManager.DeleteCase(id);

        Pause(deleted ? "Case deleted successfully." : "Case not found.");
    }

    private SeverityLevel ChooseSeverity()
    {
        while (true)
        {
            Console.WriteLine("SEVERITY LEVEL");
            Console.WriteLine("Choose how serious the issue is:");
            Console.WriteLine("1. Low - Annoying, but computer mostly works");
            Console.WriteLine("2. Medium - Problem affects normal use");
            Console.WriteLine("3. High - Computer is difficult to use");
            Console.WriteLine("4. Critical - Computer will not boot or is unusable");

            int choice = ReadInt("Severity choice: ");

            if (choice >= 1 && choice <= 4)
            {
                return (SeverityLevel)choice;
            }

            Console.WriteLine("Please choose a valid severity level.");
        }
    }

    private void DisplayCase(ComputerCase computerCase)
    {
        Console.WriteLine($"ID: {computerCase.Id}");
        Console.WriteLine($"Computer: {computerCase.ComputerName}");
        Console.WriteLine($"Device Type: {computerCase.DeviceType}");
        Console.WriteLine($"Operating System: {computerCase.OperatingSystem}");
        Console.WriteLine($"Problem: {computerCase.MainProblem}");
        Console.WriteLine($"Severity: {computerCase.Severity}");
        Console.WriteLine($"Status: {computerCase.Status}");
        Console.WriteLine($"Created: {computerCase.CreatedDate.ToShortDateString()}");
        Console.WriteLine("---------------------------------------");
    }

    private void DisplayCaseSummary()
    {
        List<ComputerCase> cases = _caseManager.GetAllCases();

        if (!cases.Any())
        {
            Console.WriteLine("No cases have been added yet.");
            return;
        }

        foreach (ComputerCase computerCase in cases)
        {
            Console.WriteLine($"{computerCase.Id}. {computerCase.ComputerName} | {computerCase.MainProblem} | {computerCase.Status}");
        }
    }

    private string? ReadInputOrCancel(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (input == "0")
        {
            return null;
        }

        return input ?? "";
    }

    private int ReadInt(string prompt)
    {
        int number;

        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out number))
            {
                return number;
            }

            Console.WriteLine("Please enter a valid number.");
        }
    }

    private void Pause(string message = "Press Enter to continue.")
    {
        Console.WriteLine($"\n{message}");
        Console.ReadLine();
    }
}