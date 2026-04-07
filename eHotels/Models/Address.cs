class Address{
    //primary key
    public string PostalCode{get;set;}

    //other keys
    public int StreetNum{get; set;}
    public string StreetName{get; set;}
    public string Province{get;set;}
    public string Country{get;set;}
    public string City{get;set;}

    public Address(){}
    public Address(string PostalCode, int StreetNum, string StreetName, string Province, string Country, string City){
        this.PostalCode = PostalCode;
        this.StreetNum = StreetNum;
        this.StreetName = StreetName;
        this.Province = Province;
        this.Country = Country;
        this.City = City;
    }

}