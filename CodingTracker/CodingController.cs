using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Text;

namespace CodingTracker;

internal class CodingController
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private readonly string? _connectionString;

    public CodingController()
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    internal void Post(Coding coding)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var insert = @"INSERT INTO coding (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
                connection.Execute(insert, coding);
            }

            AnsiConsole.Write("New record was succesfully added! Press any key to continue... ");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }

    internal void Get()
    {
        try
        {
            List<Coding> tableData = new List<Coding>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var getAll = "SELECT * FROM coding";
                tableData = connection.Query<Coding>(getAll).ToList();
            }

            TableVisualisation.ShowTable(tableData);
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }

    internal Coding? GetById(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var get = $"SELECT * FROM coding WHERE Id = @Id";
                return connection.QuerySingle<Coding>(get, new { Id = id });
            }
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
        return null;
    }

    internal void Delete(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var delete = $"DELETE FROM coding WHERE Id = @Id";
                connection.Execute(delete, new { Id = id });
            }
            AnsiConsole.Write($"Record with {id} was succesfully deleted! Press any key to continue... ");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }

    internal void Update(Coding coding)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var insert = @"UPDATE coding SET 
                                StartTime = @StartTime,
                                EndTime = @EndTime,
                                Duration = @Duration
                            WHERE
                                Id = @Id";
                connection.Execute(insert, coding);
            }
            AnsiConsole.WriteLine($"\nRecord with Id {coding.Id} was successfully updated! Press any key to continue...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }
}
