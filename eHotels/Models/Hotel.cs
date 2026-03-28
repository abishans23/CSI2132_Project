class Hotel{
    //primary key
    int hotelID; 

    //other keys
    int chainID {get;set;} //references HotelChain
    string name {get;set;}
    int stars {get;set;}
    string manager {get;set;} //references Employees
    string postalCode {get;set;} //references Address
    string description {get;set;}
    string fileName {get;set;}
    string imageDesc {get;set;}

    //MORE TO COME

    public Hotel(int chainID, string name, int stars, string manager,string postalCode, string descriptio, string fileName, string imageDesc){
        this.chainID = chainID;
        this.name = name;
        this.stars = stars;
        this.manager = manager;
        this.postalCode = postalCode;
        this.description = description;
        this.fileName = fileName;
        this.imageDesc = imageDesc;
    }

}