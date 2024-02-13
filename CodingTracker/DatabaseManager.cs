using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace CodingTracker;

internal class DatabaseManager
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    private readonly string? _connectionString;
    public DatabaseManager()
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    internal void CreateTable()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                    connection.Open();

                    var createTable =
                        @"CREATE TABLE IF NOT EXISTS coding(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT
                        )";

                    connection.Execute(createTable);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
    }
}
