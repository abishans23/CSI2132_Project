using System.Security.Principal;

class ArchivedRenting
{
    public int RentingID{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public int hotelId{get;set;}
    public string status{get;set;}

    public ArchivedRenting(){}

    public ArchivedRenting(int rentingID, DateOnly startDate, DateOnly endDate, int hotelId, string status)
    {
        this.RentingID = rentingID;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.hotelId = hotelId;
        this.status = status;
    }
}