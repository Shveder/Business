namespace Business_monitoring.DTO;

public class AddOfferRequest
{
    public int NumberOfShares { get; set; }
    public double PriceOfShare { get; set; }
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    
}