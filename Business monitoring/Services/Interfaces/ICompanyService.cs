using Business_monitoring.DTO;

namespace Business_monitoring.Services.Interfaces;

public interface ICompanyService
{
    public Task AddBusiness(AddBusinessRequest request);
    public Task ChangeBusinessPrice(ChangeBusinessPriceRequest request);
}