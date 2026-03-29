class RoomAmenity
{
    public int RoomNumber{get;set;}
    public int HotelID{get;set;}
    public string Amenity{get;set;}

    public RoomAmenity(){}

    public RoomAmenity(int roomNumber, int hotelID, string amenity)
    {
        this.RoomNumber = roomNumber;
        this.HotelID = hotelID;
        this.Amenity = amenity;
    }
}