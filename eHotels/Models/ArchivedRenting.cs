using System.Security.Principal;

class ArchivedRenting
{
    public int ArchivedRentingID{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public int HotelId{get;set;}
    public string Status{get;set;}

    public ArchivedRenting(){}

    public ArchivedRenting(int rentingID, DateOnly startDate, DateOnly endDate, int hotelId, string status)
    {
        this.ArchivedRentingID = rentingID;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.HotelId = hotelId;
        this.Status = status;
    }
}