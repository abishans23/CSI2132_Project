public class HotelPhone
{
    public int HotelID{get;set;}

    public int PhoneNumber{get;set;}

    public HotelPhone(){}

    public HotelPhone(int HotelID,int PhoneNumber)
    {
       this.HotelID=HotelID;
       this.PhoneNumber=PhoneNumber; 
    }
}