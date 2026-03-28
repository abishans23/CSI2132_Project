class HotelChain{
    //primary key
    int chainID; 

    //other keys
    string name {get;set;}
    string postalCode {get;set;} //references Address

    public HotelChain(int chainID, string name, string postalCode){
        this.chainID = chainID;
        this.name = name;
        this.postalCode = postalCode;
    }

}