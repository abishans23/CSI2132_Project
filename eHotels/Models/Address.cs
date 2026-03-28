class Address{
    //primary key
    string postalCode;

    //other keys
    int id {get;set;}
    string streetName{get;set;}
    string province{get;set;}
    string country{get;set;}

     
    public Address(string postalCode, int id, string streetName, string province){
        this.postalCode = postalCode;
        this.id = id;
        this.streetName = streetName;
        this.province = province;
    }

}