namespace Business_monitoring.DTO;

public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    public LoginRequest(string login, string password)
    {
        Login = login;
        Password = password;
    }
}