class Review
{   
    //primary keys
    public string Email{get;set;}
    public int HotelID{get;set;}

    //other keys
    public int Rating{get;set;}
    public DateOnly Date{get;set;}
    public String Comments{get;set;}

    public Review(){}
    public Review(string email, int hotelID, int rating, DateOnly date, string comments)
    {
        this.Email = email;
        this.HotelID = hotelID;
        this.Rating = rating;
        this.Date = date;
        this.Comments = comments;
    }
}