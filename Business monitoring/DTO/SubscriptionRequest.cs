namespace Business_monitoring.DTO;

public class SubscriptionRequest
{
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }

    public SubscriptionRequest(Guid userId, Guid businessId)
    {
        UserId = userId;
        BusinessId = businessId;
    }
}