public class HotelChainPhone
{
    public uint ChainID{get;set;}

    public string PhoneNumber{get;set;}

    public HotelChainPhone(){}

    public HotelChainPhone(uint ChainID,string PhoneNumber)
    {
        this.ChainID=ChainID;
        this.PhoneNumber=PhoneNumber;
    }
}