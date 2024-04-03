namespace Business_monitoring.DTO;

public class BuyExpertViewRequest
{
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }

    public BuyExpertViewRequest(Guid userId, Guid businessId)
    {
        UserId = userId;
        BusinessId = businessId;
    }
}