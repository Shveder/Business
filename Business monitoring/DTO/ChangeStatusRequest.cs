namespace Business_monitoring.DTO;

public class ChangeStatusRequest
{
    public Guid Id { get; set; }
    public bool status { get; set; }

    public ChangeStatusRequest(Guid id, bool status)
    {
        Id = id;
        this.status = status;
    }
}