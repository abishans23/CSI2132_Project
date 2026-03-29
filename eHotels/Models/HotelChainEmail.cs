using System.ComponentModel.DataAnnotations;

public class HotelChainEmail
{
    public uint ChainID{get;set;}

    //make in email format
    [Required]
    [EmailAddress]
    public string Email{get;set;}

    public HotelChainEmail(){}

    public HotelChainEmail(uint ChainID,string Email)
    {
        this.ChainID=ChainID;
        this.Email=Email;
    }
}