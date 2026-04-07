public class HotelPhone
{
    public int HotelID{get;set;}

    public string PhoneNumber{get;set;}

    public HotelPhone(){}

    public HotelPhone(int HotelID,string PhoneNumber)
    {
       this.HotelID=HotelID;
       this.PhoneNumber=PhoneNumber; 
    }
}