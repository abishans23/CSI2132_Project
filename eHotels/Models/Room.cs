class Room{

    //primary key
    int hotelID //references hotel
    int roomNumber

    //other keys
    float price {get;set;}
    int capacity {get;set;}
    string view {get;set;}
    bool extendable {get;set;}

    public Room(int hotelID, int roomNumber, float price, int capacity, string view, bool extendable){
        this.hotelID = hotelID;
        this.roomNumber = roomNumber;
        this.price = price;
        this.capacity = capacity;
        this.view = view;
        this.extendable = extendable;
    }
}