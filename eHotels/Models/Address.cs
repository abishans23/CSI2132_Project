class Address{
    //primary key
    public string PostalCode;

    //other keys
    public int StreetNumber{get; set;}
    public string StreetName{get; set;}
    public string Province{get;set;}
    public string Country{get;set;}

    public Address(){}
    public Address(string postalCode, int StreetNumber, string StreetName, string Province){
        this.PostalCode = postalCode;
        this.StreetNumber = StreetNumber;
        this.StreetName = StreetName;
        this.Province = Province;
    }

}