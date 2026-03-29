class Review
{   
    //primary keys
    public string Email;
    public int HotelID;

    //other keys
    public int Rating;
    public DateOnly Date;
    public String Comments;

    public Review(string email, int hotelID, int rating, DateOnly date, string comments)
    {
        this.Email = email;
        this.HotelID = hotelID;
        this.Rating = rating;
        this.Date = date;
        this.Comments = comments;
    }
}