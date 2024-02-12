using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal class TableVisualisation
{
    internal static void ShowTable(List<Coding> tableData)
    {
        var table = new Table();

        table.Title = new TableTitle("CODING RECORDS", "bold");

        table.AddColumns($"[{Color.Olive}]Id[/]", $"[{Color.Olive}]Start Time[/]", $"[{Color.Olive}]End Time[/]", $"[{Color.Olive}]Duration[/]").Centered();

        foreach (var c in tableData)
        {
            table.AddRow(c.Id.ToString(), $"{c.StartTime}", $"{c.EndTime}", $"{c.Duration} hours").Centered();
        }

        AnsiConsole.Write(table);
    }
}
