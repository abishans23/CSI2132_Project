using Npgsql;
using Dapper;
using System.Text.Json;

namespace Data
{
    public class DBContext
    {
        private NpgsqlDataSource? db;
        private readonly ILogger<DBContext> _logger;

        public DBContext(ILogger<DBContext> logger)
        {
            _logger = logger;
            // Initialize the DataSource immediately so it's ready for testing
            string connectionString = "Host=localhost;Port=5432;Database=eHotels;Username=postgres;Password=1234";
            db = NpgsqlDataSource.Create(connectionString);
        }

        public async Task<bool> OpenConnection()
        {
            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                if (db == null) return false;

                await using var conn = await db.OpenConnectionAsync();
                return true;
            }, _logger);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await Utils.TryExecuteAsync<IEnumerable<T>, DBContext>(async () =>
            {
                // Ensure the data source exists before querying
                if (db == null) throw new InvalidOperationException("Database source not initialized.");

                await using var conn = await db.OpenConnectionAsync();
                return await conn.QueryAsync<T>(sql, parameters);
            }, _logger) ?? Enumerable.Empty<T>();
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await Utils.TryExecuteAsync<int, DBContext>(async () =>
            {
                if (db == null) throw new InvalidOperationException("Database source not initialized.");

                await using var conn = await db.OpenConnectionAsync();
                return await conn.ExecuteAsync(sql, parameters);
            }, _logger);
        }

        public async Task<bool> SeedDatabase()
        {
            string[] jsonPaths ={
                Path.Combine("Data", "Hotel.json"),
                Path.Combine("Data", "HotelChain.json"),
                Path.Combine("Data", "Room.json"),
                Path.Combine("Data", "Account.json")
            };

            string[] fileData = new string[4];
            for (int i = 0; i < fileData.Length; i++)
            {
                fileData[i] = File.ReadAllText(jsonPaths[i]);
                if (string.IsNullOrEmpty(fileData[i]))
                {
                    _logger.LogError($"[Seed Error]: {jsonPaths[i]} is null or empty");
                }
            }

            //Container data structures for deserialized data
            var hotels = JsonSerializer.Deserialize<List<Hotel>>(fileData[0]);
            var hotelChains = JsonSerializer.Deserialize<List<HotelChain>>(fileData[1]);
            var rooms = JsonSerializer.Deserialize<List<Room>>(fileData[2]);
            var accounts = JsonSerializer.Deserialize<List<Account>>(fileData[3]);

            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                return true;

            }, _logger);
        }

    }
}