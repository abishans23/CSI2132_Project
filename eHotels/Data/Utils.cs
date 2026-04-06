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

        //build Update SQL querries TODO::IF THERE IS A BUG MAYBE HERE
        public static string BuildUpdate(string table,Dictionary<string, string> columns,IEnumerable<string> primaryKeys)
        {
            var setColumns = columns
                .Where(c => !primaryKeys.Contains(c.Key, StringComparer.OrdinalIgnoreCase))
                .Select(c => $"{c.Key} = @{c.Key}");

            string setClause = string.Join(", ", setColumns);

            var whereColumns = primaryKeys
                .Select(k => $"{k} = @{k}");

            string whereClause = string.Join(" AND ", whereColumns);

            return $"UPDATE {table} SET {setClause} WHERE {whereClause}";
        }


    }
}