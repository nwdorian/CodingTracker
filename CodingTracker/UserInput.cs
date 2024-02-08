
using Spectre.Console;
using static CodingTracker.Models.Enums;

namespace CodingTracker;

internal class UserInput
{
    internal static void MainMenu()
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
                .AddChoices(MenuSelection.ViewRecords,
                MenuSelection.AddRecord,
                MenuSelection.UpdateRecord,
                MenuSelection.DeleteRecord)
                );
        }
    }
}
