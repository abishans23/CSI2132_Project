class Booking
{
    public int BookingID{get;set;}
    public DateOnly BookingDate{get;set;}
    public string Status{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public int HotelID { get; set; }
    public int RoomNumber { get; set; }
    public string IDType{get;set;}
    public string IdNumber{get;set;}

    public Booking(){}

    public Booking(int bookingID, DateOnly bookingDate, string status, DateOnly startDate, DateOnly endDate, int roomNumber, int hotelID, string IDType, string IDNumber)
    {
        this.BookingID = bookingID;
        this.BookingDate = bookingDate;
        this.Status = status;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.RoomNumber = roomNumber;
        this.HotelID = hotelID;
        this.IDType = IDType;
        this.IdNumber = IDNumber;
    }
}