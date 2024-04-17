using Business_monitoring.DTO;
using Business_monitoring.Models;

namespace Business_monitoring.Services.Interfaces;

public interface ICompanyService
{
    public Task AddBusiness(AddBusinessRequest request);
    public Task ChangeBusinessPrice(ChangeBusinessPriceRequest request);
    public Task AddGainOfCompany(AddGainRequest request);
    public Task<IQueryable<GainsOfCompany>> GetGainsOfBusinesses(Guid id);
    public Task SetNumberOfSharesToSell(SetNumberOfSharesRequest request);
}