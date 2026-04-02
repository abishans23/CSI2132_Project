using Npgsql;
using Dapper;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            string connectionString = "Host=ep-sweet-glitter-a8ag8fj1-pooler.eastus2.azure.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_cU7jafXmtI5k; SSL Mode=VerifyFull; Channel Binding=Require;Include Error Detail=true;";
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
                Path.Combine("Data", "Account.json"),
                Path.Combine("Data", "Employee.json"),
                Path.Combine("Data", "Address.json")
            };

            List<string> fileData = new List<string>();
            foreach (var path in jsonPaths)
            {
                if (!File.Exists(path)) return false;
                fileData.Add(File.ReadAllText(path));
            }

            var options = new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            }; 

            var hotels = JsonSerializer.Deserialize<List<Hotel>>(fileData[0], options);
            var hotelChains = JsonSerializer.Deserialize<List<HotelChain>>(fileData[1], options);
            var rooms = JsonSerializer.Deserialize<List<Room>>(fileData[2], options);
            var accounts = JsonSerializer.Deserialize<List<Account>>(fileData[3], options);
            var employees = JsonSerializer.Deserialize<List<Employee>>(fileData[4], options);
            var addresses = JsonSerializer.Deserialize<List<Address>>(fileData[5], options);

            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                if (hotelChains == null || rooms == null || hotels == null || accounts == null || employees==null)
                {
                    return false;
                }

                await this.ExecuteAsync("ALTER TABLE Address ALTER COLUMN PostalCode TYPE VARCHAR(10);");
                await this.ExecuteAsync("ALTER TABLE HotelChain ALTER COLUMN PostalCode TYPE VARCHAR(10);");
                await this.ExecuteAsync("ALTER TABLE Hotel ALTER COLUMN PostalCode TYPE VARCHAR(10);");
                await this.ExecuteAsync("ALTER TABLE Employee ALTER COLUMN PostalCode TYPE VARCHAR(10);");

                await this.ExecuteAsync(CreateString.createAddress);
                await this.ExecuteAsync(CreateString.createAccount);
                await this.ExecuteAsync(CreateString.createHotelChain);
                await this.ExecuteAsync(CreateString.createHotel);
                await this.ExecuteAsync(CreateString.createEmployee);
                await this.ExecuteAsync(CreateString.createRoom);
                await this.ExecuteAsync(CreateString.createBooking);
                await this.ExecuteAsync(CreateString.createRenting);
                await this.ExecuteAsync(CreateString.createCustomer);


                await this.ExecuteAsync(@"
                    ALTER TABLE Hotel DROP CONSTRAINT IF EXISTS fk_hotel_manager;
                    ALTER TABLE Hotel ADD CONSTRAINT fk_hotel_manager FOREIGN KEY (Manager) REFERENCES Employee(SSN);
                    ALTER TABLE Employee DROP CONSTRAINT IF EXISTS fk_employee_hotel;
                    ALTER TABLE Employee ADD CONSTRAINT fk_employee_hotel FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID);");


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

                foreach (var chain in hotelChains)
                {
                    await this.ExecuteAsync(@"INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country) 
                                              VALUES (0, 'Unknown', @PostalCode, 'Unknown', 'Unknown') 
                                              ON CONFLICT (PostalCode) DO NOTHING;",
                                              new { chain.PostalCode });

                    await this.ExecuteAsync(@"INSERT INTO HotelChain (ChainID, Name, PostalCode) 
                                              VALUES (@ChainID, @Name, @PostalCode)
                                              ON CONFLICT (ChainID) DO NOTHING;",
                                              new { chain.ChainID, chain.Name, chain.PostalCode });
                }

                foreach (var acc in accounts)
                {
                    await this.ExecuteAsync(@"INSERT INTO Account (Email, Username, Password) 
                                              VALUES (@Email, @Username, @Password)
                                              ON CONFLICT (Email) DO NOTHING;",
                                              new { acc.Email, acc.Username, acc.Password });
                }


                foreach (var emp in employees)
                {
                    await this.ExecuteAsync(@"INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country) 
                                              VALUES (0, 'Unknown', @PostalCode, 'Unknown', 'Unknown') 
                                              ON CONFLICT (PostalCode) DO NOTHING;",
                                              new { emp.PostalCode });

                    await this.ExecuteAsync(@"INSERT INTO Employee (SSN, FirstName, LastName, PostalCode, Position, HotelID, Email) 
                                              VALUES (@SSN, @FirstName, @LastName, @PostalCode, @Position, @HotelID, @Email)
                                              ON CONFLICT (SSN) DO NOTHING;",
                                              new { emp.SSN, emp.FirstName, emp.LastName, emp.PostalCode, emp.Position, emp.HotelID, emp.Email });
                }

                foreach (var hotel in hotels)
                {
                    await this.ExecuteAsync(@"INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country) 
                                              VALUES (0, 'Unknown', @PostalCode, 'Unknown', 'Unknown') 
                                              ON CONFLICT (PostalCode) DO NOTHING;",
                                              new { hotel.PostalCode });

                    await this.ExecuteAsync(@"INSERT INTO Hotel (HotelID, ChainID, Name, PostalCode, Stars, Manager, Description) 
                                              VALUES (@HotelID, @ChainID, @Name, @PostalCode, @Stars, @Manager, @Description)
                                              ON CONFLICT (HotelID) DO NOTHING;",
                                              new { hotel.HotelID, hotel.ChainID, hotel.Name, hotel.PostalCode, hotel.Stars, hotel.Manager, hotel.Description });
                }

                await using var conn = await db.OpenConnectionAsync();

                foreach (var room in rooms)
                {
                    await this.ExecuteAsync(@"INSERT INTO Room (RoomNumber, HotelID, Price, Capacity, View, Extendable) 
                                              VALUES (@RoomNumber, @HotelID, @Price, @Capacity, @View, @Extendable)
                                              ON CONFLICT (RoomNumber, HotelID) DO NOTHING;",
                                              new { room.RoomNumber, room.HotelID, room.Price, room.Capacity, room.View, room.Extendable });
                }

                return true;
            }, _logger);
        }
    }
}