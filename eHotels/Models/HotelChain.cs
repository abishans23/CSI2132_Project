public class HotelChain
{
    public int ChainID { get; set; }
    public string Name { get; set; }
    public string PostalCode { get; set; }

    public HotelChain() { }

    public HotelChain(int chainID, string name, string postalCode)
    {
        this.ChainID = chainID;
        this.Name = name;
        this.PostalCode = postalCode;
    }
}