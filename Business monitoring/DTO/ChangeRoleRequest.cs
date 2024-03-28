namespace Business_monitoring.DTO;

public class ChangeRoleRequest
{
    public Guid Id { get; set; }
    public int Role { get; set; }

    public ChangeRoleRequest(Guid id, int role)
    {
        Id = id;
        Role = role;
    }
}