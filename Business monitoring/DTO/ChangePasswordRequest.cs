namespace Business_monitoring.DTO;

public class ChangePasswordRequest
{
    public Guid Id { get; set; }
    public string PreviousPassword { get; set; }
    public string NewPassword { get; set; }

    public ChangePasswordRequest(Guid id, string previousPassword, string newPassword)
    {
        Id = id;
        PreviousPassword = previousPassword;
        NewPassword = newPassword;
    }
}