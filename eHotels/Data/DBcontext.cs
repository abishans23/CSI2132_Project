using Npgsql;
using Dapper;
using Microsoft.Extensions.Logging;

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

        // Keep this for your manual connection tests (e.g., in a Startup check)
        public async Task<bool> OpenConnection()
        {
            return await Utils.TryExecute<bool, DBContext>(async () =>
            {
                if (db == null) return false;

                await using var conn = await db.OpenConnectionAsync();
                return true;
            }, _logger);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await Utils.TryExecute<IEnumerable<T>, DBContext>(async () =>
            {
                // Ensure the data source exists before querying
                if (db == null) throw new InvalidOperationException("Database source not initialized.");

                await using var conn = await db.OpenConnectionAsync();
                return await conn.QueryAsync<T>(sql, parameters);
            }, _logger) ?? Enumerable.Empty<T>();
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await Utils.TryExecute<int, DBContext>(async () =>
            {
                if (db == null) throw new InvalidOperationException("Database source not initialized.");

                await using var conn = await db.OpenConnectionAsync();
                return await conn.ExecuteAsync(sql, parameters);
            }, _logger);
        }
    }
}