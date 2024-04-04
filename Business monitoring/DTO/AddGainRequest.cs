namespace Business_monitoring.DTO;

public class AddGainRequest
{
    public Guid BusinessId { get; set; }
    public string Year { get; set; }
    public double Gain { get; set; }

    public AddGainRequest(Guid businessId, string year, double gain)
    {
        BusinessId = businessId;
        Year = year;
        Gain = gain;
    }
}