class Employee{

    //TODO::Mark fields public

    //primary key
    public string SSN{get;set;}

    //other keys
    string FirstName {get;set;}
    string LastName {get;set;}
    string PostalCode {get;set;} //references address
    string Position {get;set;}
    string HotelID {get;set;} //references hotel
    string Email {get;set;} //references account

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