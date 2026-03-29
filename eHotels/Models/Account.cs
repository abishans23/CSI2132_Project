public class Account
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public Account() { }

    public Account(string email, string username, string password)
    {
        this.Email = email;
        this.Username = username;
        this.Password = password;
    }
}