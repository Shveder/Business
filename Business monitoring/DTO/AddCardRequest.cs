namespace Business_monitoring.DTO;

public class AddCardRequest
{
    public string Number { get; set; }
    public string Date { get; set; }
    public string Name { get; set; }
    public string Cvv { get; set; }
    public Guid Id { get; set; }

    public AddCardRequest(string number, string date, string name, string cvv, Guid id)
    {
        Number = number;
        Date = date;
        Name = name;
        Cvv = cvv;
        Id = id;
    }
}