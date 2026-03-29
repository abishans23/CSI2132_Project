class CustBooking
{

    public int BookingID{get;set;}
    public string IDType{get;set;}
    public int IDNumber{get;set;}

    public CustBooking(){}

    public CustBooking(int bookingID, string idType, int idNumber)
    {
        this.BookingID =bookingID;
        this.IDType = idType;
        this.IDNumber = idNumber;
    }
}