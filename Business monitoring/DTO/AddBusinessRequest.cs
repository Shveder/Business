namespace Business_monitoring.DTO;

public class AddBusinessRequest
{
    public string Name { get; set; }
    public double PriceOfCompany { get; set; }
    public double PriceOfShare { get; set; }
    public double ExpertViewPrice { get; set; }
    public Guid CompanyId { get; set; }

    public AddBusinessRequest(string name, double priceOfCompany, double priceOfShare, double expertViewPrice, Guid companyId)
    {
        Name = name;
        PriceOfCompany = priceOfCompany;
        PriceOfShare = priceOfShare;
        ExpertViewPrice = expertViewPrice;
        CompanyId = companyId;
    }
}