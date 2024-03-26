namespace Business_monitoring.DTO;

public class CreateUserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
    public int Role { get; set; }//1-user/2-company/3-expert
    public string Phone { get; set; }
    public string Email { get; set; }

    public CreateUserRequest(string login, string password, string repeatPassword, int role, string phone, string email)
    {
        Login = login;
        Password = password;
        RepeatPassword = repeatPassword;
        Role = role;
        Phone = phone;
        Email = email;
    }
}