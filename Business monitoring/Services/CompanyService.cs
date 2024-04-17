using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;

namespace Business_monitoring.Services;

public class CompanyService : ICompanyService
{
    private readonly IDbRepository _repository;
    private readonly ISubscriptionService _subscriptionService;

    public CompanyService(IDbRepository repository, ISubscriptionService subscriptionService)
    {
        _repository = repository;
        _subscriptionService = subscriptionService;
    }

    public async Task AddBusiness(AddBusinessRequest request)
    {
        if (request.Name.Length < 4 || request.Name.Length > 100)
            throw new IncorrectDataException("Длина имени должна быть от 4 до 99 символов");
        if (request.PriceOfCompany < 0)
            throw new IncorrectDataException("Цена компании должна быть положительной");
        if (request.PriceOfShare < 0)
            throw new IncorrectDataException("Цена акции должна быть положительной");
        if (request.ExpertViewPrice < 0)
            throw new IncorrectDataException("Цена мнения эксперта должна быть положительной");
        var company = GetCompanyById(request.CompanyId);
        
        var business = new Business()
            {
                Name = request.Name,
                PriceOfCompany = request.PriceOfCompany,
                PriceOfShare = request.PriceOfShare,
                ExpertViewPrice = request.ExpertViewPrice,
                NumberOfShares = 1000,
                NumberToSell = 0,
                Company = company
            };
        await _repository.Add(business);
        await _repository.SaveChangesAsync();
            
    }
    
    private Company GetCompanyById(Guid id)
    {
        var company = _repository.Get<Company>(model => model.Id == id).FirstOrDefault();
        if (company == null)
            throw new IncorrectDataException("Нет компании с таким id");
        return company;
    }
    private Business GetBusinessById(Guid id)
    {
        var business = _repository.Get<Business>(model => model.Id == id).FirstOrDefault();
        if (business == null)
            throw new IncorrectDataException("Нет бизнеса с таким id");
        return business;
    }

    public async Task ChangeBusinessPrice(ChangeBusinessPriceRequest request)
    {
        if (request.NewPrice < 0)
            throw new IncorrectDataException("Цена должна быть положительной");
        var business = GetBusinessById(request.BusinessId);

        var recentPrice = new RecentPricesOfBusiness()
        {
            Business = business,
            Price = request.NewPrice
        };
        business.PriceOfCompany = request.NewPrice;
        business.DateUpdated = DateTime.UtcNow;
    
        var subs = await _subscriptionService.GetAllSubscribers(business.Id);

        foreach (var sub in subs)
        {
            await _subscriptionService.Notify(sub.User.Id,
                $"Цена бизнеса {business.Name}" +
                $" изменилась и составила {business.PriceOfCompany}");
        }
    
        await _repository.Update(business);
        await _repository.Add(recentPrice);
        await _repository.SaveChangesAsync();
    }

    public async Task AddGainOfCompany(AddGainRequest request)
    {
        var business = GetBusinessById(request.BusinessId);
        
        var gains = await GetGainsOfBusinesses(request.BusinessId);
        if (!int.TryParse(request.Year, out int year))
        {
            throw new IncorrectDataException("Некорректный формат года.");
        }

        var currentYear = DateTime.Now.Year;
    
        // Проверка, чтобы год не был больше текущего
        if (year > currentYear)
        {
            throw new IncorrectDataException("Указанный год больше текущего года.");
        }

        if (gains.Any(g => g.Year == request.Year))
            throw new IncorrectDataException("Год уже существует в списке результатов прибыли для данного бизнеса.");

        var gain = new GainsOfCompany()
        {
            Business = business,
            Year = request.Year,
            Gain = request.Gain
        };

        await _repository.Add(gain);
        await _repository.SaveChangesAsync();

    }
    
    public Task<IQueryable<GainsOfCompany>> GetGainsOfBusinesses(Guid id)
    {
        var business = GetBusinessById(id);
        return Task.FromResult(_repository.Get<GainsOfCompany>(model => model.Business == business));
    }

    public async Task SetNumberOfSharesToSell(SetNumberOfSharesRequest request)
    {
        var business = GetBusinessById(request.BusinessId);
        if (request.NumberOfShares > business.NumberOfShares || request.NumberOfShares < 0)
            throw new IncorrectDataException("Invalid number of Shares");
        business.NumberToSell = request.NumberOfShares;
        business.DateUpdated = DateTime.UtcNow;
        await _repository.Update(business);
        await _repository.SaveChangesAsync();
    }
}  
