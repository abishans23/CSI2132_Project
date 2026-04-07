public class HotelChainPhone
{
    public int ChainID{get;set;}

    public string PhoneNumber{get;set;}

    public HotelChainPhone(){}

    public HotelChainPhone(int ChainID,string PhoneNumber)
    {
        this.ChainID=ChainID;
        this.PhoneNumber=PhoneNumber;
    }
}