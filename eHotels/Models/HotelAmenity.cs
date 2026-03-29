public class HotelAmenity
{
    public string AmenityName { get; set; }
    public string AmenityDesc { get; set; }

    public HotelAmenity(){}
    public HotelAmenity(string amenityName,string amenityDesc)
    {
        this.AmenityName=amenityName;
        this.AmenityDesc=amenityDesc;
    }
}