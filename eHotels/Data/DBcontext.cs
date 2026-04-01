using Npgsql;
using Dapper;
using System.Net.Sockets;
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
            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                if (db == null) return false;

                try
                {
                    await db.OpenConnectionAsync();
                }
                catch (Npgsql.NpgsqlException ex) when (ex.InnerException is SocketException socketEx)
                {
                    Console.WriteLine($"DB connection failed: {socketEx.SocketErrorCode}");
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    Console.WriteLine($"Npgsql error: {ex.Message}");
                }
                return true;
            }, _logger);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await Utils.TryExecuteAsync<IEnumerable<T>, DBContext>(async () =>
            {
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
            string[] jsonPaths = {
                Path.Combine("Data", "Hotel.json"),
                Path.Combine("Data", "HotelChain.json"),
                Path.Combine("Data", "Room.json"),
                Path.Combine("Data", "Account.json")
            };

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

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var hotels = JsonSerializer.Deserialize<List<Hotel>>(fileData[0], options);
            var hotelChains = JsonSerializer.Deserialize<List<HotelChain>>(fileData[1], options);
            var rooms = JsonSerializer.Deserialize<List<Room>>(fileData[2], options);
            var accounts = JsonSerializer.Deserialize<List<Account>>(fileData[3], options);

            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                if (hotelChains == null || rooms == null || hotels == null || accounts == null)
                {
                    _logger.LogError("One or more JSON files failed to deserialize.");
                    return false;
                }


                // Base Tables
                await this.ExecuteAsync(CreateString.createAddress);
                await this.ExecuteAsync(CreateString.createAccount);
                await this.ExecuteAsync(CreateString.createBooking);
                await this.ExecuteAsync(CreateString.createRenting);

                // Level 2
                await this.ExecuteAsync(CreateString.createHotelChain);
                await this.ExecuteAsync(CreateString.createCustomer);

                await this.ExecuteAsync(CreateString.createEmployee);
                await this.ExecuteAsync(CreateString.createHotel);
                await this.ExecuteAsync(CreateString.createRoom);

                //alter table commands to fix temporary circular dependency between Hotel and Employee
                await this.ExecuteAsync(@"
                                        ALTER TABLE Hotel 
                                        ADD CONSTRAINT fk_hotel_manager 
                                        FOREIGN KEY (Manager) REFERENCES Employee(SSN);");

                await this.ExecuteAsync(@"
                                        ALTER TABLE Employee 
                                        ADD CONSTRAINT fk_employee_hotel 
                                        FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID);");

                await this.ExecuteAsync(CreateString.createHotelEmail);
                await this.ExecuteAsync(CreateString.createHotelPhone);
                await this.ExecuteAsync(CreateString.createHotelChainEmail);
                await this.ExecuteAsync(CreateString.createHotelChainPhone);
                await this.ExecuteAsync(CreateString.createHotelImage);
                await this.ExecuteAsync(CreateString.createHotelAmenity);
                await this.ExecuteAsync(CreateString.createReview);

                await this.ExecuteAsync(CreateString.createRoomProblem);
                await this.ExecuteAsync(CreateString.createRoomAmenity);
                await this.ExecuteAsync(CreateString.createRoomBooking);
                await this.ExecuteAsync(CreateString.createRentedRoom);
                await this.ExecuteAsync(CreateString.createCustBooking);
                await this.ExecuteAsync(CreateString.createRentingTenant);
                



                // HotelChain
                
                foreach (var chain in hotelChains)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO HotelChain(ChainID, Name, PostalCode) 
                          VALUES (@ChainID, @Name, @PostalCode)
                          ON CONFLICT (ChainID) DO NOTHING;", chain);
                }

                // Hotel
                /*
                if (hotels != null)
                {
                    foreach (var hotel in hotels)
                    {

                        await this.ExecuteAsync(
                            @"INSERT INTO Hotel(HotelID, ChainID, Name, Stars, Manager, PostalCode, Description) 
                          VALUES (@HotelID, @ChainID, @Name, @Stars, @Manager, @PostalCode, @Description)
                          ON CONFLICT (HotelID) DO NOTHING;", hotel);
                    }
                }

                // Room
                foreach (var room in rooms)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO Room(HotelID, RoomNumber, Price, Capacity, View, Extendable) 
                          VALUES (@HotelID, @RoomNumber, @Price, @Capacity, @View, @Extendable)
                          ON CONFLICT (HotelID, RoomNumber) DO NOTHING;", room);
                }

                */

                // Account
                foreach (var account in accounts)
                {
                    await this.ExecuteAsync(
                        @"INSERT INTO Account(Email, Username, Password) 
                          VALUES (@Email, @Username, @Password)
                          ON CONFLICT (Email) DO NOTHING;", account);
                }


                return true;
            }, _logger);
        }
    }
}