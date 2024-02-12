using CodingTracker.Models;
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
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (StartTime, EndTime, Duration) VALUES ('{coding.StartTime}', '{coding.EndTime}', '{coding.Duration}')";
                    tableCmd.ExecuteNonQuery();
                }
            }

            AnsiConsole.Write("New record was succesfully added! Press any key to continue... ");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
    }

    internal void Get()
    {
        try
        {
            List<Coding> tableData = new List<Coding>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                    new Coding
                                    {
                                        Id = reader.GetInt32(0),
                                        StartTime = reader.GetString(1),
                                        EndTime = reader.GetString(2),
                                    });
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine("No rows found!");
                        }
                    }
                }
            }

            TableVisualisation.ShowTable(tableData);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
    }

    internal Coding? GetById(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM coding WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        Coding coding = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            coding.Id = reader.GetInt32(0);
                            coding.StartTime = reader.GetString(1);
                            coding.EndTime = reader.GetString(2);
                        }
                        return coding;
                    }
                }
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
        return null;
    }

    internal void Delete(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE FROM coding WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }
            AnsiConsole.Write($"Record with {id} was succesfully deleted! Press any key to continue... ");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
    }

    internal void Update(Coding coding)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $@"UPDATE coding SET 
                                                StartTime = '{coding.StartTime}'
                                                EndTime = '{coding.EndTime}'
                                                Duration = '{coding.Duration}'
                                            WHERE
                                                Id = '{coding.Id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }
            AnsiConsole.WriteLine($"\nRecord with Id {coding.Id} was successfully updated! Press any key to continue...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error! Details: " + e.Message);
        }
    }
}
