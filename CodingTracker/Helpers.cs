﻿using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal static class Helpers
{
    internal static string GetDateInput(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(message)
            .Validate(input =>
            {
                return (!DateTime.TryParseExact(input, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _)) ?
                ValidationResult.Error("[red]Invalid input! Please provide the following format (dd-MM-yy)[/]") : ValidationResult.Success();
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

    internal static bool ValidateDate(string startTime, string endTime)
    {
        DateTime start = DateTime.ParseExact(startTime, "dd-MM-yy H:mm", new CultureInfo("en-US"));
        DateTime end = DateTime.ParseExact(endTime, "dd-MM-yy H:mm", new CultureInfo("en-US"));
        
        if (start > end)
        {
            return false;
        }
        return true;
    }

    internal static string? SelectOrdering()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select ordering for the report:")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Ascending",
                "Descending"
            }));
    }
}
