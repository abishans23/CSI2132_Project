public class HotelChain
{
    public int ChainID { get; set; }
    public string ChainName { get; set; }
    public string ChainPostalCode { get; set; }

    public HotelChain() { }

    public HotelChain(int chainID, string ChainName, string ChainPostalCode)
    {
        this.ChainID = chainID;
        this.ChainName = ChainName;
        this.ChainPostalCode = ChainPostalCode;


    }
}