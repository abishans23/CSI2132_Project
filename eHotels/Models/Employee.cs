class Employee{

    //primary key
    string SSN;

    //other keys
    string firstName {get;set;}
    string lastName {get;set;}
    string postalCode {get;set;} //references address
    string position {get;set;}
    string hotelID {get;set;} //references hotel
    string email {get;set;} //references account

    public Employee(string SSN, string firstName, string lastName, string postalCode, string position, string hotelID, string email){
        this.SSN = SSN;
        this.firstName = firstName;
        this.lastName = lastName;
        this.postalCode = postalCode;
        this.position = position;
        this.hotelID = hotelID;
        this.email = email;
    }

}