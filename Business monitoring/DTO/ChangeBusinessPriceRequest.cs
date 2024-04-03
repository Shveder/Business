namespace Business_monitoring.DTO;

public class ChangeBusinessPriceRequest
{
    public Guid BusinessId { get; set; }
    public double NewPrice { get; set; }

    public ChangeBusinessPriceRequest(Guid businessId, double newPrice)
    {
        BusinessId = businessId;
        NewPrice = newPrice;
    }
}