namespace Business_monitoring.DTO;

public class AddViewRequest
{
    public Guid ExpertId { get; set; }
    public Guid BusinessId { get; set; }
    public string View { get; set; }
}