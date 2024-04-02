using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;

namespace Business_monitoring.Services;

public class CompanyService : ICompanyService
{
    private readonly IDbRepository _repository;

    public CompanyService(IDbRepository repository)
    {
        _repository = repository;
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
}
