using CodingTracker.Models;
using Spectre.Console;
using System.Globalization;
using System.Text;
using static CodingTracker.Models.Enums;

namespace CodingTracker;

internal class UserInput
{
    CodingController codingController = new();
    internal void MainMenu()
    {
        bool repeatMenu = true;
        while (repeatMenu)
        {
            repeatMenu = false;
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MenuSelection>()
                .Title("Welcome to [green]Coding tracker[/]\nWhat would you like to do?")
                .PageSize(10)
                .AddChoices(MenuSelection.LiveSession,
                            MenuSelection.ViewRecords,
                            MenuSelection.AddRecord,
                            MenuSelection.UpdateRecord,
                            MenuSelection.DeleteRecord,
                            MenuSelection.ViewReports,
                            MenuSelection.CloseApplication)
                            );

            switch (selection)
            {
                case MenuSelection.LiveSession:
                    ProcessLiveSession();
                    MainMenu();
                    break;
                case MenuSelection.ViewRecords:
                    codingController.Get();
                    AnsiConsole.Write("\nPress any key to continue... ");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case MenuSelection.AddRecord:
                    ProcessAdd();
                    MainMenu();
                    break;
                case MenuSelection.UpdateRecord:
                    ProcessUpdate();
                    MainMenu();
                    break;
                case MenuSelection.DeleteRecord:
                    ProcessDelete();
                    MainMenu();
                    break;
                case MenuSelection.ViewReports:
                    ReportsMenu();
                    MainMenu();
                    break;
                case MenuSelection.CloseApplication:
                    if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                    {
                        Console.WriteLine("\nGoodbye!");
                    }
                    else
                    {
                        repeatMenu = true;
                    }
                    break;
            }
        }
    }

    internal void UpdateMenu(Coding coding)
    {
        bool updating = true;
        while (updating)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<UpdatingSelection>()
                .Title("Select from the options below:")
                .PageSize(10)
                .AddChoices(UpdatingSelection.UpdateStartTime,
                            UpdatingSelection.UpdateEndTime,
                            UpdatingSelection.SaveChanges,
                            UpdatingSelection.MainMenu)
                );
            switch (selection)
            {
                case UpdatingSelection.UpdateStartTime:
                    var newStart = Helpers.GetDateInput("Please insert new start date and time (format: dd-MM-yy H:mm):");
                    coding.StartTime = DateTime.ParseExact(newStart, "dd-MM-yy H:mm", new CultureInfo("en-US"));
                    break;
                case UpdatingSelection.UpdateEndTime:
                    var newEnd = Helpers.GetDateInput("Please insert new end date and time (format: dd-MM-yy H:mm):");

                    while (!Helpers.ValidateDate(coding.StartTime.ToString("dd-MM-yy H:mm"), newEnd))
                    {
                        AnsiConsole.MarkupLine("\n[red]Invalid input! End time can't be before start time![/]\n");
                        newEnd = Helpers.GetDateInput("Please insert a valid end date and time (format: dd-MM-yy H:mm): ");
                    }

                    coding.EndTime = DateTime.ParseExact(newEnd, "dd-MM-yy H:mm", new CultureInfo("en-US"));
                    break;
                case UpdatingSelection.SaveChanges:
                    codingController.Update(coding);
                    updating = false;
                    break;
                case UpdatingSelection.MainMenu:
                    updating = false;
                    break;
            }
        }
    }

    internal void ReportsMenu()
    {
        bool repeat = true;

        while (repeat)
        {
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<ReportSelection>()
                .Title("Select from the options below:")
                .PageSize(10)
                .AddChoices(ReportSelection.Weekly,
                            ReportSelection.Monthly,
                            ReportSelection.Yearly,
                            ReportSelection.MainMenu)
                );
            switch (selection)
            {
                case ReportSelection.Weekly:
                    ProcessReport(DateTime.Now.AddDays(-7), "WEEKLY CODING REPORTS");
                    break;
                case ReportSelection.Monthly:
                    ProcessReport(DateTime.Now.AddMonths(-1), "MONTHLY CODING REPORTS");
                    break;
                case ReportSelection.Yearly:
                    ProcessReport(DateTime.Now.AddYears(-1), "YEARLY CODING REPORTS");
                    break;
                case ReportSelection.MainMenu:
                    repeat = false;
                    break;
            }
        }
    }

    private void ProcessAdd()
    {
        var startTime = Helpers.GetDateInput("Please insert the start date and time (format: dd-MM-yy H:mm): ");
        var endTime = Helpers.GetDateInput("Please insert the end date and time (format: dd-MM-yy H:mm): ");

        while (!Helpers.ValidateDate(startTime, endTime))
        {
            AnsiConsole.MarkupLine("\n[red]Invalid input! End time can't be before start time![/]\n");
            endTime = Helpers.GetDateInput("Please insert a valid end date and time (format: dd-MM-yy H:mm): ");
        }

        Coding coding = new Coding();

        coding.StartTime = DateTime.ParseExact(startTime, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None);
        coding.EndTime = DateTime.ParseExact(endTime, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None);

        codingController.Post(coding);
    }

    private void ProcessDelete()
    {
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to delete:");

        var coding = codingController.GetById(id);

        if (coding is null)
        {
            AnsiConsole.Write($"\nRecord with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessDelete();
        }
        else
        {
            codingController.Delete(coding);
        }
    }

    private void ProcessUpdate()
    {
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to update:");

        var coding = codingController.GetById(id);

        if (coding is null)
        {
            AnsiConsole.Write($"\nRecord with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessUpdate();
        }
        else
        {
            UpdateMenu(coding);
        }
    }

    private void ProcessLiveSession()
    {
        AnsiConsole.Write("Press any key to start a live session...");
        Console.ReadKey();
        AnsiConsole.MarkupLine("\n\n[green]Live session started![/]");

        Coding coding = new();
        coding.StartTime = DateTime.Now;

        AnsiConsole.Write("Press any key to stop the live session...");
        Console.ReadKey();
        AnsiConsole.MarkupLine("\n\n[red]Live session ended![/]");

        coding.EndTime = DateTime.Now;

        codingController.Post(coding);
    }

    private void ProcessReport(DateTime period, string title)
    {
        var tableData = codingController.GetByTimePeriod(period);

        var order = Helpers.SelectOrdering();

        if (order == "Ascending")
        {
            var asc = tableData?.OrderBy(s => s.StartTime).ToList();
            TableVisualisation.ShowTable(asc!, title);
        }
        else
        {
            var desc = tableData?.OrderByDescending(s => s.StartTime).ToList();
            TableVisualisation.ShowTable(desc!, title);
        }

        TimeSpan totalDuration = new TimeSpan();

        foreach (var c in tableData!)
        {
            totalDuration += c.Duration;
        }

        AnsiConsole.MarkupLineInterpolated($"\nTotal coding duration: {totalDuration.TotalHours:N0} hours : {totalDuration.Minutes} minutes");

        var average = totalDuration.Ticks / tableData.Count;
        var averageTime = new TimeSpan(average);
        AnsiConsole.MarkupLineInterpolated($"Average coding duration per session: {averageTime.Hours} hours : {averageTime.Minutes} minutes");

        AnsiConsole.Write("\nPress any key to continue...");
        Console.ReadKey();
    }
}
