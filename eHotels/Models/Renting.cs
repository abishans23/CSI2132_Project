using System.Security.Principal;

class Renting
{
    public int RentingID{get;set;}
    public string Status{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public string PaymentMethod{get;set;}
    public int Amount{get;set;}
    public DateOnly ProcessedDate{get;set;}

    public string IDType{get;set;}
    public int IDNumber{get;set;}

    public int HotelID{get;set;}
    public int RoomNumber{get;set;}

    public Renting(){}

    public Renting(int rentingID, string status, DateOnly startDate, DateOnly endDate, string paymentMethod, int amount, DateOnly processedDate, string idType, int idNumber, int hotelID, int roomNumber)
    {
        this.RentingID = rentingID;
        this.Status = status;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.PaymentMethod = paymentMethod;
        this.Amount = amount;
        this.ProcessedDate = processedDate;
        this.IDType = idType;
        this.IDNumber = idNumber;
        this.HotelID = hotelID;
        this.RoomNumber = roomNumber;
    }
}