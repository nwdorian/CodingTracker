using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal static class Helpers
{
    internal static string GetDateInput(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(message)
            .Validate(dateInput =>
            {
                return (!DateTime.TryParseExact(dateInput, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _)) ?
                ValidationResult.Error("[red]Invalid input! Please provide the following format (dd-MM-yy hh:mm)[/]") : ValidationResult.Success();
            }));
    }

    internal static int GetNumberInput(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(message)
            .ValidationErrorMessage("[red]Invalid input![/]")
            .Validate(num =>
            {
                return num switch
                {
                    <= 0 => ValidationResult.Error("[red]Number must be bigger then 0![/]"),
                    _ => ValidationResult.Success()
                };
            }));
    }

    internal static bool ValidateEndDate(string startDate, string endDate)
    {
        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);

        if (start > end)
        {
            return false;
        }
        return true;
    }
}
