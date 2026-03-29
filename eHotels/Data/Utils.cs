using Npgsql;
using Microsoft.Extensions.Logging;

namespace Data
{
    public static class Utils
    {
        public static async Task<T?> TryExecuteAsync<T,V>(Func<Task<T>> operation,ILogger<V> _logger)
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