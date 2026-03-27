using Npgsql;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class DBContext
    {
        private NpgsqlDataSource? db;   //mark as nullable type
        private readonly ILogger<DBContext> _logger;
        public DBContext(ILogger<DBContext> logger)
        {
            _logger=logger;
        }

        public async Task<bool> OpenConnection()
        {
            string connectionString = "Host=localhost;Port=5432;Database=eHotels;Username=postgres;Password=1234";
            db = NpgsqlDataSource.Create(connectionString);

            
            return await TryExecute(async () => 
            {
                await using var conn = await db.OpenConnectionAsync();
                return true; 
            });
        }

        public async Task<T?> TryExecute<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (NpgsqlException ex)
            {
                // Catches all NpgSQL errors
                _logger.LogError($"[Npgsql Error]: {ex.Message} (Code: {ex.SqlState})");
                return default; 
            }
            catch (Exception ex)
            {
                // Catches non-Postgres errors (like timeouts or network being totally down)
                _logger.LogError($"[General Error]: {ex.Message}");
                return default;
            }
        }
    }
}