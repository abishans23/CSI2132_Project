public class Room
{
    public int HotelID { get; set; }
    public int RoomNumber { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public string View { get; set; }
    public bool Extendable { get; set; }

    public Room() { }

    public Room(int hotelID, int roomNumber, decimal price, int capacity, string view, bool extendable)
    {
        this.HotelID = hotelID;
        this.RoomNumber = roomNumber;
        this.Price = price;
        this.Capacity = capacity;
        this.View = view;
        this.Extendable = extendable;
    }
}