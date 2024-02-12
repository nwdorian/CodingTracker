using CodingTracker.Models;
using Spectre.Console;
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
                    break;
                case MenuSelection.DeleteRecord:
                    ProcessDelete();
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
                .AddChoices(UpdatingSelection.StartTime,
                            UpdatingSelection.EndTime,
                            UpdatingSelection.SaveChanges,
                            UpdatingSelection.MainMenu)
                );
            switch (selection)
            {
                case UpdatingSelection.StartTime:
                    coding.StartTime = Helpers.GetDateInput("Set a new starting date and time (format: dd-MM-yy hh:mm):");
                    break;
                case UpdatingSelection.EndTime:
                    coding.EndTime = Helpers.GetDateInput("Set a new ending date and time (format: dd-MM-yy hh:mm): ");
                    break;
                case UpdatingSelection.SaveChanges:
                    updating = false;
                    codingController.Update(coding);
                    break;
                case UpdatingSelection.MainMenu:
                    updating = false;
                    MainMenu();
                    break;
            }
        }
    }

    private void ProcessAdd()
    {
        var startDate = Helpers.GetDateInput("Please insert the start date and time: (Format: dd-MM-yy hh:mm)");
        var endDate = Helpers.GetDateInput("Please insert the end date and time: (Format: dd-MM-yy hh:mm)");

        while (!Helpers.ValidateEndDate(startDate, endDate))
        {
            AnsiConsole.WriteLine("\n[red]Invalid input! End date can't be before start date![/]\n");
            endDate = Helpers.GetDateInput("Please insert the end date and time: (Format: dd-MM-yy hh:mm)");
        }

        Coding coding = new Coding();

        coding.StartTime = startDate;
        coding.EndTime = endDate;

        codingController.Post(coding);
    }

    private void ProcessDelete()
    {
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to delete:");

        var coding = codingController.GetById(id);

        if (coding?.Id == 0)
        {
            AnsiConsole.Write($"\nRecord with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessDelete();
        }
        else
        {
            codingController.Delete(id);
        }
    }

    private void ProcessUpdate()
    {
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to update:");

        var coding = codingController.GetById(id);

        if (coding?.Id == 0)
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
        Console.WriteLine("Coming soon");
    }
}
