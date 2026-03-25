class Account{
    //primary key
    string email
    

    //other keys
    string username {get;set;}
    string password {get;set;}

    public Account(string email, string username, string password){
        this.email = email;
        this.username = username;
        this.password = password;
    }

}