using System.ComponentModel.DataAnnotations;

public class HotelChainEmail
{
    public int ChainID{get;set;}

    //make in email format
    [Required]
    [EmailAddress]
    public string Email{get;set;}

    public HotelChainEmail(){}

    public HotelChainEmail(int ChainID,string Email)
    {
        this.ChainID=ChainID;
        this.Email=Email;
    }
}