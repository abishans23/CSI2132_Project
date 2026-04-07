namespace Data
{
    static class CreateString
    {
        public static readonly string createHotel = @"CREATE TABLE IF NOT EXISTS Hotel(
                    HotelID INT PRIMARY KEY CHECK(HotelID >=0),
                    ChainID INT CHECK(ChainID >= 0),
                    Name VARCHAR(50) NOT NULL,
                    PostalCode VARCHAR(10) NOT NULL,
                    Stars INT CHECK (Stars BETWEEN 1 AND 5),
                    Manager VARCHAR(20),
                    Description VARCHAR(200),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID) ON DELETE CASCADE
                    );";

        public static readonly string createHotelChain = @"CREATE TABLE IF NOT EXISTS HotelChain (
                    ChainID INT CHECK(ChainID >= 0) PRIMARY KEY,
                    ChainName VARCHAR(50) NOT NULL,
                    ChainPostalCode VARCHAR(10) NOT NULL,
                    FOREIGN KEY (ChainPostalCode) REFERENCES Address(PostalCode)
                    );";

        public static readonly string createAddress = @"CREATE TABLE IF NOT EXISTS Address (
                    StreetNum INT NOT NULL,
                    StreetName VARCHAR(50) NOT NULL,
                    PostalCode VARCHAR(10) NOT NULL,
                    Province VARCHAR(10) NOT NULL,
                    Country VARCHAR(20) NOT NULL,
                    City VARCHAR(20),
                    PRIMARY KEY(PostalCode)
                    );";

        public static readonly string createHotelEmail = @"CREATE TABLE IF NOT EXISTS HotelEmail (
                    HotelID INT CHECK(HotelID >= 0),
                    Email VARCHAR(30) CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (HotelID, Email),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createHotelPhone = @"CREATE TABLE IF NOT EXISTS HotelPhone (
                    HotelID INT,
                    PhoneNumber VARCHAR(10),
                    PRIMARY KEY (HotelID, PhoneNumber),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createHotelChainEmail = @"CREATE TABLE IF NOT EXISTS HotelChainEmail (
                    ChainID INT CHECK (ChainID >= 0),
                    Email VARCHAR(30) CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (ChainID, Email),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID) ON DELETE CASCADE
                    );";

        public static readonly string createHotelChainPhone = @"CREATE TABLE IF NOT EXISTS HotelChainPhone (
                    ChainID INT CHECK (ChainID >= 0),
                    PhoneNumber VARCHAR(10),
                    PRIMARY KEY (ChainID, PhoneNumber),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID) ON DELETE CASCADE
                    );";

        public static readonly string createHotelImage = @"CREATE TABLE IF NOT EXISTS HotelImage(
                    HotelID INT CHECK(HotelID >= 0),
                    FileName VARCHAR(25),
                    ImageDesc VARCHAR(100),
                    PRIMARY KEY (HotelID, FileName),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID) ON DELETE CASCADE
                    );";


        public static readonly string createAccount = @"CREATE TABLE IF NOT EXISTS Account(
                    Email VARCHAR(30) PRIMARY KEY CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    Username VARCHAR(20) NOT NULL,
                    Password VARCHAR(15) NOT NULL
                    );";

        public static readonly string createEmployee = @"CREATE TABLE IF NOT EXISTS Employee(
                    SSN VARCHAR(20) PRIMARY KEY,
                    FirstName VARCHAR(20) NOT NULL,
                    LastName VARCHAR(20) NOT NULL,
                    PostalCode VARCHAR(10) NOT NULL,
                    Position VARCHAR(20) CHECK(Position IN('Manager', 'Concierge', 'Receptionist', 'Cleaning', 'Restaurant')) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Email VARCHAR(30) NOT NULL CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode),
                    FOREIGN KEY (Email) REFERENCES Account(Email) ON DELETE CASCADE
                    );";

        public static readonly string createRoom = @"CREATE TABLE IF NOT EXISTS Room(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Price DECIMAL(8, 2) CHECK(Price >= 0),
                    Capacity INT CHECK(Capacity BETWEEN 1 AND 6),
                    View VARCHAR(30),
                    Extendable BOOL,
                    PRIMARY KEY (RoomNumber, HotelID),
                    FOREIGN KEY (HotelID) REFERENCES Hotel (HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createRoomProblem = @"CREATE TABLE IF NOT EXISTS RoomProblem(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Problem VARCHAR(40),
                    PRIMARY KEY (RoomNumber, HotelID, Problem),
                    FOREIGN KEY (RoomNumber, HotelID) REFERENCES Room(RoomNumber, HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createRoomAmenity = @"CREATE TABLE IF NOT EXISTS RoomAmenity(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Amenity VARCHAR(40),
                    PRIMARY KEY (RoomNumber, HotelID, Amenity),
                    FOREIGN KEY (RoomNumber, HotelID) REFERENCES Room(RoomNumber, HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createBooking = @"CREATE TABLE IF NOT EXISTS Booking(
                    BookingID INT NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
                    BookingDate DATE NOT NULL,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Cancelled', 'Scheduled')),
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL,
                    RoomNumber INT NOT NULL,
                    HotelID INT NOT NULL,
                    IDType VARCHAR(30) NOT NULL,
                    IDNumber VARCHAR(30) NOT NULL,
                    CHECK (StartDate <= EndDate),
                    FOREIGN KEY(RoomNumber, HotelID) REFERENCES Room(RoomNumber, HotelID) ON DELETE CASCADE,
                    FOREIGN KEY(IDType, IDNumber) REFERENCES Customer (IDType, IDNumber) ON DELETE CASCADE
                    );";


        public static readonly string createCustomer = @"CREATE TABLE IF NOT EXISTS Customer(
                    IDType VARCHAR(30),
                    IDNumber VARCHAR(30),
                    FirstName VARCHAR(20) NOT NULL,
                    LastName VARCHAR(20) NOT NULL,
                    RegistrationDate DATE NOT NULL,
                    PhoneNumber VARCHAR(10) NOT NULL,
                    PostalCode VARCHAR(10) NOT NULL,
                    Email VARCHAR(30) CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (IDType, IDNumber),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode)
                    );";



        public static readonly string createRenting = @"CREATE TABLE IF NOT EXISTS Renting(
                    RentingID INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Occupied', 'Cancelled', 'Finished')),
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL,
                    PaymentMethod VARCHAR(20) NOT NULL,
                    Amount INT NOT NULL,
                    ProcessedDate DATE NOT NULL,
                    RoomNumber INT NOT NULL,
                    HotelID INT NOT NULL,
                    IDType VARCHAR(30) NOT NULL,
                    IDNumber VARCHAR(30) NOT NULL,
                    CHECK (StartDate <= EndDate),
                    FOREIGN KEY(RoomNumber, HotelID) REFERENCES Room(RoomNUmber, HotelID) ON DELETE CASCADE,
                    FOREIGN KEY(IDType, IDNumber) REFERENCES Customer (IDType, IDNumber) ON DELETE CASCADE
                    );";



        public static readonly string createReview = @"CREATE TABLE IF NOT EXISTS Review(
                    Email VARCHAR(30) NOT NULL CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    HotelID INT CHECK(HotelID >= 0),
                    Rating INT CHECK(Rating BETWEEN 1 AND 5),
                    Date DATE,
                    Comments VARCHAR(200),
                    PRIMARY KEY(Email, HotelID),
                    FOREIGN KEY (Email) REFERENCES Account(Email) ON DELETE CASCADE,
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID) ON DELETE CASCADE
                    );";

        public static readonly string createArchivedBooking = @"CREATE TABLE IF NOT EXISTS ArchivedBooking(
	                ArchivedBookingID INT NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
                    HotelID INT CHECK(HotelID >=0),
                    BookingDate DATE,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Cancelled', 'Scheduled')),
                    StartDate DATE,
                    EndDate DATE,
                    CHECK(StartDate <= EndDate),
                    FOREIGN KEY(HotelID) REFERENCES Hotel ON DELETE CASCADE
                );";

        public static readonly string createArchivedRenting = @"CREATE TABLE IF NOT EXISTS ArchivedRenting(
                    ArchivedRentingID INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Occupied', 'Cancelled', 'Finished')),
                    HotelID INT CHECK(HotelID >=0),
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL,
                    CHECK(StartDate<=EndDate),
                    FOREIGN KEY(HotelID) REFERENCES Hotel ON DELETE CASCADE
                );";
    }
}