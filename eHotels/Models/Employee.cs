class Employee{

    //TODO::Mark fields public

    //primary key
    public string SSN{get;set;}

    //other keys
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string PostalCode {get;set;} //references address
    public string Position {get;set;}
    public string HotelID {get;set;} //references hotel
    public string Email {get;set;} //references account

    public Employee(){}
    public Employee(string SSN, string firstName, string lastName, string postalCode, string position, string hotelID, string email){
        this.SSN = SSN;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.PostalCode = postalCode;
        this.Position = position;
        this.HotelID = hotelID;
        this.Email = email;
    }

}