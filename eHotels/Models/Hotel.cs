class Hotel{
    //primary key
    int hotelID 

    //other keys
    int chainID {get;set;} //references HotelChain
    string name {get;set;}
    int stars {get;set;}
    int manager {get;set;} //references Employees
    string postalCode {get;set;} //references Address

    public Account(int chainID, string name, string postalCode){
        this.email = email;
        this.username = username;
        this.password = password;
    }

}