namespace Business_monitoring.DTO;

public class DepositRequest
{
    public Guid Id { get; set; }
    public double Sum { get; set; }

    public DepositRequest(Guid id, double sum)
    {
        Id = id;
        Sum = sum;
    }
}