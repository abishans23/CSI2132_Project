namespace Data
{
    static class IndexString
    {
        public static readonly string area = @"CREATE INDEX area on Address(City)";

        public static readonly string employeesInHotel = @"CREATE INDEX employeesInHotel on Employee(HotelID)";

        public static readonly string roomcapacity = @"CREATE INDEX roomCapacity on Room(Capacity)";

        public static readonly string bookingdates = @"CREATE INDEX bookingDates ON Booking(HotelID, RoomNumber, StartDate, EndDate";

    }

}