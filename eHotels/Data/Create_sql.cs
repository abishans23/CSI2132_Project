namespace Data
{
    static class CreateString
    {
        public static readonly string createHotel = @"CREATE TABLE IF NOT EXISTS Hotel(
                    HotelID INT PRIMARY KEY CHECK(HotelID >=0),
                    ChainID INT CHECK(ChainID >= 0),
                    Name VARCHAR(50) NOT NULL,
                    PostalCode VARCHAR(6) NOT NULL,
                    Stars INT CHECK (Stars BETWEEN 1 AND 5),
                    Manager VARCHAR(9) NOT NULL,
                    Description VARCHAR(200),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID)
                    );";

        public static readonly string createHotelChain = @"CREATE TABLE IF NOT EXISTS HotelChain (
                    ChainId INT CHECK(ChainID >= 0) PRIMARY KEY,
                    Name VARCHAR(50) NOT NULL,
                    PostalCode VARCHAR(6) NOT NULL,
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode)
                    );";

        public static readonly string createAddress = @"CREATE TABLE IF NOT EXISTS Address (
                    StreetNum INT NOT NULL,
                    StreetName VARCHAR(50) NOT NULL,
                    PostalCode VARCHAR(6) NOT NULL,
                    Province VARCHAR(10) NOT NULL,
                    Country VARCHAR(20) NOT NULL,
                    PRIMARY KEY(PostalCode)
                    );";

        public static readonly string createHotelEmail = @"CREATE TABLE IF NOT EXISTS HotelEmail (
                    HotelID INT CHECK(HotelID >= 0),
                    Email VARCHAR(30) CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (HotelID, Email),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID)
                    );";

        public static readonly string createHotelPhone = @"CREATE TABLE IF NOT EXISTS HotelPhone (
                    HotelID INT,
                    PhoneNumber VARCHAR(10),
                    PRIMARY KEY (HotelID, PhoneNumber),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID)
                    );";

        public static readonly string createHotelChainEmail = @"CREATE TABLE IF NOT EXISTS HotelChainEmail (
                    ChainID INT CHECK (ChainID >= 0),
                    Email VARCHAR(30) CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (ChainID, Email),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID)
                    );";

        public static readonly string createHotelChainPhone = @"CREATE TABLE IF NOT EXISTS HotelChainPhone (
                    ChainID INT CHECK (ChainID >= 0),
                    PhoneNumber VARCHAR(10),
                    PRIMARY KEY (ChainID, PhoneNumber),
                    FOREIGN KEY (ChainID) REFERENCES HotelChain(ChainID)
                    );";

        public static readonly string createHotelImage = @"CREATE TABLE IF NOT EXISTS HotelImage(
                    HotelID INT CHECK(HotelID >= 0),
                    FileName VARCHAR(25),
                    ImageDesc VARCHAR(100),
                    PRIMARY KEY (HotelID, FileName)
                    );";

        public static readonly string createHotelAmenity = @"CREATE TABLE IF NOT EXISTS HotelAmenity(
                    HotelID INT CHECK(HotelID >= 0),
                    AmenityName VARCHAR(20),
                    AmenityDesc VARCHAR(100),
                    PRIMARY KEY (HotelID, AmenityName)
                    );";

        public static readonly string createAccount = @"CREATE TABLE IF NOT EXISTS Account(
                    Email VARCHAR(30) PRIMARY KEY CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    Username VARCHAR(20) NOT NULL,
                    Password VARCHAR(15) NOT NULL
                    );";

        public static readonly string createEmployee = @"CREATE TABLE IF NOT EXISTS Employee(
                    SSN VARCHAR(9) PRIMARY KEY,
                    FirstName VARCHAR(20) NOT NULL,
                    LastName VARCHAR(20) NOT NULL,
                    PostalCode VARCHAR(6) NOT NULL,
                    Position VARCHAR(20) CHECK(Position IN('Manager', 'Concierge', 'Receptionist', 'Cleaning', 'Restaurant')) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Email VARCHAR(30) NOT NULL CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode),
                    FOREIGN KEY (Email) REFERENCES Account(Email)
                    );";

        public static readonly string createRoom = @"CREATE TABLE IF NOT EXISTS Room(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Price DECIMAL(8, 2) CHECK(Price >= 0),
                    Capacity INT CHECK(Capacity BETWEEN 1 AND 6),
                    View VARCHAR(30),
                    Extendable BOOL,
                    PRIMARY KEY (RoomNumber, HotelID),
                    FOREIGN KEY (HotelID) REFERENCES Hotel (HotelID)
                    );";

        public static readonly string createRoomProblem = @"CREATE TABLE IF NOT EXISTS RoomProblem(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Problem VARCHAR(40),
                    PRIMARY KEY (RoomNumber, HotelID, Problem),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID),
                    FOREIGN KEY (RoomNumber) REFERENCES Room (RoomNumber)
                    );";

        public static readonly string createRoomAmenity = @"CREATE TABLE IF NOT EXISTS RoomAmenity(
                    RoomNumber INT CHECK(RoomNumber >= 0) NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    Amenity VARCHAR(40),
                    PRIMARY KEY (RoomNumber, HotelID, Amenity),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID),
                    FOREIGN KEY (RoomNumber) REFERENCES Room (RoomNumber)
                    );";

        public static readonly string createBooking = @"CREATE TABLE IF NOT EXISTS Booking(
                    BookingID INT NOT NULL CHECK (BookingID >= 0) PRIMARY KEY,
                    BookingDate DATE NOT NULL,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Cancelled', 'Scheduled', 'Occupied')),
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL
                    );";

        public static readonly string createRoomBooking = @"CREATE TABLE IF NOT EXISTS RoomBooking(
                    BookingID INT NOT NULL CHECK (BookingID >= 0),
                    RoomNumber INT NOT NULL,
                    HotelID INT NOT NULL CHECK(HotelID >= 0),
                    PRIMARY KEY (BookingID, RoomNumber, HotelID),
                    FOREIGN KEY(RoomNumber, HotelID) REFERENCES Room(RoomNumber, HotelID),
                    FOREIGN KEY (BookingID) REFERENCES Booking(BookingID)
                    );";

        public static readonly string createCustomer = @"CREATE TABLE IF NOT EXISTS Customer(
                    IDType VARCHAR(30),
                    IDNumber VARCHAR(30),
                    FirstName VARCHAR(20) NOT NULL,
                    LastName VARCHAR(20) NOT NULL,
                    RegistrationDate DATE NOT NULL,
                    PhoneNumber VARCHAR(10) NOT NULL,
                    PostalCode VARCHAR(6) NOT NULL,
                    Email VARCHAR(30) NOT NULL CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    PRIMARY KEY (IDType, IDNumber),
                    FOREIGN KEY (PostalCode) REFERENCES Address(PostalCode)
                    );";

        public static readonly string createCustBooking = @"CREATE TABLE IF NOT EXISTS CustBooking(
                    BookingID INT CHECK (BookingID >= 0),
                    IDType VARCHAR(30),
                    IDNumber VARCHAR(30),
                    PRIMARY KEY (BookingID, IDType, IDNumber),
                    FOREIGN KEY (BookingID) REFERENCES Booking(BookingID),
                    FOREIGN KEY(IDType, IDNumber) REFERENCES Customer (IDType, IDNumber)
                    );";

        public static readonly string createRenting = @"CREATE TABLE IF NOT EXISTS Renting(
                    RentingID INT CHECK(RentingID >=0) PRIMARY KEY,
                    Status VARCHAR(20) NOT NULL CHECK(Status IN ('Occupied', 'Cancelled', 'Finished')),
                    StartDate DATE NOT NULL,
                    EndDate DATE NOT NULL,
                    InvoiceNumber INT,
                    PaymentMethod VARCHAR(20) NOT NULL,
                    Amount INT NOT NULL,
                    ProcessedDate DATE NOT NULL
                    );";

        public static readonly string createRentingTenant = @"CREATE TABLE IF NOT EXISTS RentingTenant(
                    RentingID INT CHECK(RentingID >= 0),
                    IDType VARCHAR(30),
                    IDNumber VARCHAR(30),
                    PRIMARY KEY(RentingID, IDType, IDNumber),
                    FOREIGN KEY(RentingID) REFERENCES Renting(RentingID),
                    FOREIGN KEY(IDType, IDNumber) REFERENCES Customer (IDType, IDNumber)
                    );";

        public static readonly string createRentedRoom = @"CREATE TABLE IF NOT EXISTS RentedRoom(
                    RentingID INT CHECK(RentingID >= 0),
                    RoomNumber INT,
                    HotelID INT CHECK(HotelID >= 0),
                    PRIMARY KEY (RentingID, RoomNumber, HotelID),
                    FOREIGN KEY(RoomNumber, HotelID) REFERENCES Room (RoomNumber, HotelID),
                    FOREIGN KEY(RentingID) REFERENCES Renting(RentingID)
                    );";

        public static readonly string createReview = @"CREATE TABLE IF NOT EXISTS Review(
                    Email VARCHAR(30) NOT NULL CHECK(Email LIKE '%@%' AND Email LIKE '%.%'),
                    HotelID INT CHECK(HotelID >= 0),
                    Rating INT CHECK(Rating BETWEEN 1 AND 5),
                    Date DATE,
                    Comments VARCHAR(200),
                    PRIMARY KEY(Email, HotelID),
                    FOREIGN KEY (Email) REFERENCES Customer(Email),
                    FOREIGN KEY (HotelID) REFERENCES Hotel(HotelID)
                    );";
    }
}