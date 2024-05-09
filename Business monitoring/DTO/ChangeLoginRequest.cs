namespace Business_monitoring.DTO;

public class ChangeLoginRequest
{
    public Guid Id { get; set; }
    public string NewLogin { get; set; }

    public ChangeLoginRequest(Guid id, string newLogin)
    {
        Id = id;
        NewLogin = newLogin;
    }
}