namespace Business_monitoring.DTO;

public class CreateUserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }

    public CreateUserRequest(string login, string password, string repeatPassword)
    {
        Login = login;
        Password = password;
        RepeatPassword = repeatPassword;
    }
}