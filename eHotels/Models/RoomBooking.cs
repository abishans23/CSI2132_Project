class RoomBooking
{
    public int BookingID{get;set;}
    public int RoomNumber{get;set;}
    public int HotelID{get;set;}

    public RoomBooking(){}

    public RoomBooking(int bookingID, int roomNumber, int hotelID)
    {
        this.BookingID = bookingID;
        this.RoomNumber = roomNumber;
        this.HotelID = hotelID;
    }
}