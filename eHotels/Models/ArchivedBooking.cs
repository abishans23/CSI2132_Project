using System.Runtime.InteropServices;
using Data;

class ArchivedBooking
{
    public int BookingID{get;set;}
    public DateOnly BookingDate{get;set;}
    public string Status{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public int hotelId{get;set;}

    public ArchivedBooking(){}

    public ArchivedBooking(int bookingID, DateOnly bookingDate, string status, DateOnly startDate, DateOnly endDate, int hotelId)
    {
        this.BookingID = bookingID;
        this.BookingDate = bookingDate;
        this.Status = status;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.hotelId = hotelId;
    }
}