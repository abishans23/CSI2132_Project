using System.ComponentModel.DataAnnotations;

public class HotelEmail
{
    public int HotelID{get;set;}

    //Make sure field is in email address format
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email{get;set;}

    public HotelEmail(){}

    public HotelEmail(int HotelID,string Email)
    {
        this.HotelID=HotelID;
        this.Email=Email;
    }
}