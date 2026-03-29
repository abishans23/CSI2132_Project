public class Amenity
{
    public string amenityName { get; set; }
    public string amenityDesc { get; set; }

    public Amenity(string amenityName,string amenityDesc)
    {
        this.amenityName=amenityName;
        this.amenityDesc=amenityDesc;
    }
}