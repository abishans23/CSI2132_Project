public class HotelChain
{
    public int ChainID { get; set; }
    public string ChainName { get; set; }
    public string PostalCode { get; set; }

    public HotelChain() { }

    public HotelChain(int chainID, string chainName, string postalCode)
    {
        this.ChainID = chainID;
        this.ChainName = chainName;
        this.PostalCode = postalCode;
    }
}