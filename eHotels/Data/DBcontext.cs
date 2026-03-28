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

    
            return await Utils.TryExecute<bool,DBContext>(async () => 
            {
                await using var conn = await db.OpenConnectionAsync();
                return true; 
            },_logger);
        }

    }
}