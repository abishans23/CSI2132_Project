using Npgsql;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Data
{
    public static class Utils
    {
        public static async Task<T?> TryExecuteAsync<T, V>(Func<Task<T>> operation, ILogger<V> _logger)
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

        public static async Task<Dictionary<string, string>> MapSchemaToDictionary(DBContext db, string sql)
        {
            var result = await db.QueryAsync<dynamic>(sql, null);
            return result.ToDictionary(
                row => (string)row.column_name,
                row => (string)row.data_type
            );
        }

        //build Update SQL querries
        public static async Task<string> BuildUpdate(string table, Dictionary<string, string> columns)
        {
            //string idValue = columns["id"]; // extract id
            int idValue=1;

            var setColumns = columns
                .Where(c => c.Key.ToLower() != "id")
                .Select(c => $"{c.Key} = @{c.Key}");

            string setClause = string.Join(", ", setColumns);

            return $"UPDATE {table} SET {setClause} WHERE id = {idValue}";
        }


    }
}