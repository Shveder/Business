namespace Business_monitoring.DTO;

public class SetNumberOfSharesRequest
{
    public Guid BusinessId { get; set; }
    public int NumberOfShares { get; set; }
}