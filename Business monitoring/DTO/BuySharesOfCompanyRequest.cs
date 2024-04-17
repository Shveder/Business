namespace Business_monitoring.DTO;

public class BuySharesOfCompanyRequest
{
    public Guid BusinessId { get; set; }
    public int NumberOfShares { get; set; }
    public Guid UserId { get; set; }
}