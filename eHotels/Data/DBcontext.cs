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
            string connectionString = "Host=ep-sweet-glitter-a8ag8fj1-pooler.eastus2.azure.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_cU7jafXmtI5k; SSL Mode=VerifyFull; Channel Binding=Require;";
            db = NpgsqlDataSource.Create(connectionString);
        }

        public async Task<bool> OpenConnection()
        {
            await SeedDatabase();
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

        //TODO::move SeedDatabase to a better place

        //SeedDataBase populates the database with JSON file data on startup or empty database

        public async Task<bool> SeedDatabase()
        {
            string[] jsonPaths = {
                Path.Combine("Data", "Hotel.json"),
                Path.Combine("Data", "HotelChain.json"),
                Path.Combine("Data", "Room.json"),
                Path.Combine("Data", "Account.json")
            };

            // Load and check files
            List<string> fileData = new List<string>();
            foreach (var path in jsonPaths)
            {
                if (!File.Exists(path))
                {
                    _logger.LogError($"[Seed Error]: File not found at {path}");
                    return false;
                }
                fileData.Add(File.ReadAllText(path));
            }

            // Deserialize data

            var options =  new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            //TODO::instantiate all models

            //TODO::create all JSON files
            var hotels = JsonSerializer.Deserialize<List<Hotel>>(fileData[0],options);
            var hotelChains = JsonSerializer.Deserialize<List<HotelChain>>(fileData[1],options);
            var rooms = JsonSerializer.Deserialize<List<Room>>(fileData[2],options);
            var accounts = JsonSerializer.Deserialize<List<Account>>(fileData[3],options);

            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                if (hotelChains == null || rooms == null)
                {
                    _logger.LogError("One or more JSON files failed to deserialize.");
                    return false;
                }


                // 1. Insert HotelChains (Must be first if Hotels reference them)
                foreach (var chain in hotelChains)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO HotelChains (ChainID, Name, PostalCode) 
                  VALUES (@ChainID, @Name, @PostalCode)
                  ON CONFLICT (ChainID) DO NOTHING;", chain);
                }

                // 2. Insert Hotels
                
                foreach (var hotel in hotels)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO Hotels (HotelID, ChainID, Name, Stars, Manager, PostalCode, Description, FileName, ImageDesc) 
                  VALUES (@HotelID, @ChainID, @Name, @Stars, @Manager, @PostalCode, @Description, @FileName, @ImageDesc)
                  ON CONFLICT (HotelID) DO NOTHING;", hotel);
                }
                

                // 3. Insert Rooms
                foreach (var room in rooms)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO Rooms (HotelID, RoomNumber, Price, Capacity, View, Extendable) 
                  VALUES (@HotelID, @RoomNumber, @Price, @Capacity, @View, @Extendable)
                  ON CONFLICT (HotelID, RoomNumber) DO NOTHING;", room);
                }


                // 4. Insert Accounts
                
                foreach (var account in accounts)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO Accounts (Email, Username, Password) 
                  VALUES (@Email, @Username, @Password)
                  ON CONFLICT (Email) DO NOTHING;", account);
                }


                return true;
            }, _logger);
        }
    }
}