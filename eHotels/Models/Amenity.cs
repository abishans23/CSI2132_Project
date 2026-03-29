public class Amenity
{
    public string AmenityName { get; set; }
    public string AmenityDesc { get; set; }

    public Amenity(){}
    public Amenity(string amenityName,string amenityDesc)
    {
        this.AmenityName=amenityName;
        this.AmenityDesc=amenityDesc;
    }
}