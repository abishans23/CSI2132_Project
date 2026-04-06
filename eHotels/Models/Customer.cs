class Customer{

    //primary keys
    public string IDType{get;set;}
    public int IdNumber{get;set;}

    //other keys
    public string FirstName{get;set;}
    public string LastName{get;set;}
    public DateOnly RegistrationDate{get;set;}
    public int PhoneNumber{get;set;}
    public string PostalCode{get;set;}
    public string Email{get;set;}

    public Customer(){}
    public Customer(string idType, int idNumber, string firstName, string lastName, DateOnly registrationDate, int phoneNumber, string postalCode, string email)
    {
        this.IDType = idType;
        this.IdNumber = idNumber;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.RegistrationDate = registrationDate;
        this.PhoneNumber = phoneNumber;
        this.PostalCode = postalCode;
        this.Email = email;
    }
    
    
}