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
            // Note: In a production environment, move this to appsettings.json or Environment Variables
            string connectionString = "Host=ep-sweet-glitter-a8ag8fj1-pooler.eastus2.azure.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_cU7jafXmtI5k; SSL Mode=VerifyFull; Channel Binding=Require;Include Error Detail=true;";
            db = NpgsqlDataSource.Create(connectionString);
        }

        public async Task<bool> OpenConnection()
        {
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
            // 1. Load JSON Data
            string[] jsonPaths = {
                Path.Combine("Data", "Hotel.json"),
                Path.Combine("Data", "HotelChain.json"),
                Path.Combine("Data", "Room.json"),
                Path.Combine("Data", "Account.json"),
                Path.Combine("Data", "Employee.json"),
                Path.Combine("Data", "Address.json")
            };

            var fileData = new List<string>();
            foreach (var path in jsonPaths)
            {
                if (!File.Exists(path)) return false;
                fileData.Add(File.ReadAllText(path));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var hotels = JsonSerializer.Deserialize<List<Hotel>>(fileData[0], options);
            var hotelChains = JsonSerializer.Deserialize<List<HotelChain>>(fileData[1], options);
            var rooms = JsonSerializer.Deserialize<List<Room>>(fileData[2], options);
            var accounts = JsonSerializer.Deserialize<List<Account>>(fileData[3], options);
            var employees = JsonSerializer.Deserialize<List<Employee>>(fileData[4], options);

            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                // 2. Create Tables in Dependency Order
                // Independent tables first
                // await ExecuteAsync(CreateString.createAddress);
                // await ExecuteAsync(CreateString.createAccount);
                // await ExecuteAsync(CreateString.createCustomer); // Must exist before Booking/Renting
                
                // // Tables with Foreign Keys
                // await ExecuteAsync(CreateString.createHotelChain);
                // await ExecuteAsync(CreateString.createHotel);
                // await ExecuteAsync(CreateString.createEmployee);
                // await ExecuteAsync(CreateString.createRoom);
                
                // // Transactional tables
                // await ExecuteAsync(CreateString.createBooking);
                // await ExecuteAsync(CreateString.createRenting);
                
                // // Metadata/Attribute tables
                // await ExecuteAsync(CreateString.createHotelEmail);
                // await ExecuteAsync(CreateString.createHotelPhone);
                // await ExecuteAsync(CreateString.createHotelChainEmail);
                // await ExecuteAsync(CreateString.createHotelChainPhone);
                // await ExecuteAsync(CreateString.createHotelImage);
                // await ExecuteAsync(CreateString.createReview);
                // await ExecuteAsync(CreateString.createRoomProblem);
                // await ExecuteAsync(CreateString.createRoomAmenity);
                // await ExecuteAsync(CreateString.createArchivedBooking);
                // await ExecuteAsync(CreateString.createArchivedRenting);

                // // 3. Create Programmability Objects
                 await ExecuteAsync(TriggerString.bookingconflict);
                // await ExecuteAsync(TriggerString.deletebooking);
                 await ExecuteAsync(TriggerString.rentingconflict);

                // await ExecuteAsync(IndexString.area);
                // await ExecuteAsync(IndexString.bookingdates);
                // await ExecuteAsync(IndexString.employeesInHotel);
                // await ExecuteAsync(IndexString.roomcapacity);

                // await ExecuteAsync(ViewString.RoomNum);
                // await ExecuteAsync(ViewString.RoomNumCity);

                // 4. Data Insertion
                // foreach (var chain in hotelChains ?? new())
                // {
                //     await ExecuteAsync(@"
                //         INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country, City)
                //         VALUES (0, 'Unknown', @ChainPostalCode, 'Unknown', 'Unknown', 'Unknown')
                //         ON CONFLICT (PostalCode) DO NOTHING;", new { chain.ChainPostalCode });

                //     await ExecuteAsync(@"
                //         INSERT INTO HotelChain (ChainID, ChainName, ChainPostalCode)
                //         VALUES (@ChainID, @ChainName, @ChainPostalCode)
                //         ON CONFLICT (ChainID) DO NOTHING;", new { chain.ChainID, chain.ChainName, chain.ChainPostalCode });
                // }

                // foreach (var acc in accounts ?? new())
                // {
                //     await ExecuteAsync(@"
                //         INSERT INTO Account (Email, Username, Password)
                //         VALUES (@Email, @Username, @Password)
                //         ON CONFLICT (Email) DO NOTHING;", new { acc.Email, acc.Username, acc.Password });
                // }

                // foreach (var hotel in hotels ?? new())
                // {
                //     await ExecuteAsync(@"
                //         INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country, City)
                //         VALUES (0, 'Unknown', @PostalCode, 'Unknown', 'Unknown', 'Unknown')
                //         ON CONFLICT (PostalCode) DO NOTHING;", new { hotel.PostalCode });

                //     await ExecuteAsync(@"
                //         INSERT INTO Hotel (HotelID, ChainID, Name, PostalCode, Stars, Manager, Description)
                //         VALUES (@HotelID, @ChainID, @Name, @PostalCode, @Stars, NULL, @Description)
                //         ON CONFLICT (HotelID) DO NOTHING;", new { hotel.HotelID, hotel.ChainID, hotel.Name, hotel.PostalCode, hotel.Stars, hotel.Description });
                // }

                // foreach (var emp in employees ?? new())
                // {
                //     await ExecuteAsync(@"
                //         INSERT INTO Address (StreetNum, StreetName, PostalCode, Province, Country, City)
                //         VALUES (0, 'Unknown', @PostalCode, 'Unknown', 'Unknown', 'Unknown')
                //         ON CONFLICT (PostalCode) DO NOTHING;", new { emp.PostalCode });

                //     await ExecuteAsync(@"
                //         INSERT INTO Employee (SSN, FirstName, LastName, PostalCode, Position, HotelID, Email)
                //         VALUES (@SSN, @FirstName, @LastName, @PostalCode, @Position, @HotelID, @Email)
                //         ON CONFLICT (SSN) DO NOTHING;", new { emp.SSN, emp.FirstName, emp.LastName, emp.PostalCode, emp.Position, emp.HotelID, emp.Email });
                // }

                // foreach (var room in rooms ?? new())
                // {
                //     await ExecuteAsync(@"
                //         INSERT INTO Room (RoomNumber, HotelID, Price, Capacity, View, Extendable)
                //         VALUES (@RoomNumber, @HotelID, @Price, @Capacity, @View, @Extendable)
                //         ON CONFLICT (RoomNumber, HotelID) DO NOTHING;", new { room.RoomNumber, room.HotelID, room.Price, room.Capacity, room.View, room.Extendable });
                // }

                // // 5. Finalize Relationships (Circular dependencies)
                // await ExecuteAsync(@"
                //     ALTER TABLE Hotel DROP CONSTRAINT IF EXISTS fk_hotel_manager;
                //     ALTER TABLE Hotel ADD CONSTRAINT fk_hotel_manager FOREIGN KEY (Manager) REFERENCES Employee(SSN);
                //     ALTER TABLE Employee DROP CONSTRAINT IF EXISTS fk_employee_hotel;
                //     ALTER TABLE Employee ADD CONSTRAINT fk_employee_hotel FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID);");

                // foreach (var hotel in hotels ?? new())
                // {
                //     if (!string.IsNullOrEmpty(hotel.Manager))
                //     {
                //         await ExecuteAsync(@"UPDATE Hotel SET Manager = @Manager WHERE HotelID = @HotelID;", 
                //             new { hotel.Manager, hotel.HotelID });
                //     }
                // }

                return true;
            }, _logger);
        }

        public async Task<bool> GetColumnsAndTypes()
        {
            return await Utils.TryExecuteAsync<bool, DBContext>(async () =>
            {
                TableColumnsAndTypes.Hotel = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotel);
                TableColumnsAndTypes.HotelChain = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelChain);
                TableColumnsAndTypes.Address = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetAddress);
                TableColumnsAndTypes.HotelEmail = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelEmail);
                TableColumnsAndTypes.HotelPhone = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelPhone);
                TableColumnsAndTypes.HotelChainEmail = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelChainEmail);
                TableColumnsAndTypes.HotelChainPhone = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelChainPhone);
                TableColumnsAndTypes.HotelImage = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetHotelImage);
                TableColumnsAndTypes.Account = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetAccount);
                TableColumnsAndTypes.Employee = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetEmployee);
                TableColumnsAndTypes.Room = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetRoom);
                TableColumnsAndTypes.RoomProblem = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetRoomProblem);
                TableColumnsAndTypes.RoomAmenity = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetRoomAmenity);
                TableColumnsAndTypes.Booking = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetBooking);
                TableColumnsAndTypes.Customer = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetCustomer);
                TableColumnsAndTypes.Renting = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetRenting);
                TableColumnsAndTypes.Review = await Utils.MapSchemaToDictionary(this, ColumnsAndTypes.GetReview);
                return true;
            }, _logger);
        }
    }
}