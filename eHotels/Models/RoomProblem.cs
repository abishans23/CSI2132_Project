class RoomProblem
{

    public int RoomNumber{get;set;}
    public int HotelID{get;set;}
    public string Problem{get;set;}

    public RoomProblem(){}

    public RoomProblem(int roomNumber, int hotelID, string problem)
    {
        this.RoomNumber = roomNumber;
        this.HotelID = hotelID;
        this.Problem = problem;
    }
}