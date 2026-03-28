using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using System.IO;
using System.Threading.Tasks;

class SqlCreate
{
    static async Task Main(string[] args)
    {
        await Create();
    }

    static async Task Create()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connectionString);
        try
        {
            await conn.OpenAsync();
            Console.WriteLine("Connection established");

            await using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS HotelChain (
                        ChainId INT PRIMARY KEY,
                        Name VARCHAR(50),
                        EmailAddress VARCHAR(50),
                        PhoneNumber VARCHAR(10)
                    );";

                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Finished creating HotelChain table.");

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Address (
                        PostalCode VARCHAR(6) PRIMARY KEY,
                        StreetNum INT,
                        StreetName VARCHAR(50),
                        Province VARCHAR(10),
                        Country VARCHAR(20)
                    );";

                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Finished creating Address table.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection failed.");
            Console.WriteLine(e.Message);
        }

        async Task ReadDataAsync(NpgsqlConnection conn, string title)
        {
            Console.WriteLine($"\n--- {title} ---");
            await using var cmd = new NpgsqlCommand("SELECT * FROM books ORDER BY publication_year;", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            var books = new StringBuilder();
            while (await reader.ReadAsync())
            {
                books.AppendLine(
                    $"ID: {reader.GetInt32(0)}, " +
                    $"Title: {reader.GetString(1)}, " +
                    $"Author: {reader.GetString(2)}, " +
                    $"Year: {reader.GetInt32(3)}, " +
                    $"In Stock: {reader.GetBoolean(4)}"
                );
            }

            Console.WriteLine(books.ToString().TrimEnd());
            Console.WriteLine("--------------------\n");
        }
    }
}