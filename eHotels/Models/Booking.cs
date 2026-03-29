class Booking
{
    public int BookingID{get;set;}
    public DateOnly BookingDate{get;set;}
    public string Status{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}

    public Booking(){}

    public Booking(int bookingID, DateOnly bookingDate, string status, DateOnly startDate, DateOnly endDate)
    {
        this.BookingID = bookingID;
        this.BookingDate = bookingDate;
        this.Status = status;
        this.StartDate = startDate;
        this.EndDate = endDate;
    }
}