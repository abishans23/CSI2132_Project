using Npgsql;

namespace Data
{
    class DBContext
    {
        private NpgsqlDataSource db;
        public DBContext() { }

        public async Task<bool> openConnection()
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
                Console.WriteLine($"[Npgsql Error]: {ex.Message} (Code: {ex.SqlState})");
                return default; 
            }
            catch (Exception ex)
            {
                // Catches non-Postgres errors (like timeouts or network being totally down)
                Console.WriteLine($"[General Error]: {ex.Message}");
                return default;
            }
        }
    }
}