using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;

namespace Business_monitoring.Services;

public class ExpertService : IExpertService
{
    private readonly IDbRepository _repository;

    public ExpertService(IDbRepository repository)
    {
        _repository = repository;
    }

    public async Task AddExpertView(AddViewRequest request)
    {
        if (request.View.Length < 10)
            throw new IncorrectDataException("Длина мнения должна быть больше 10 символов");
        var business = GetBusinessById(request.BusinessId);
        var expert = GetExpertById(request.ExpertId);

        var expertView = new ExpertView
        {
            View = request.View,
            Business = business,
            Expert = expert
        };

        expert.Level += 1;
        expert.DateUpdated = DateTime.UtcNow;

        await _repository.Update(expert);
        await _repository.Add(expertView);
        await _repository.SaveChangesAsync();
    }
    private Business GetBusinessById(Guid id)
    {
        var business = _repository.Get<Business>(model => model.Id == id).FirstOrDefault();
        if (business == null)
            throw new IncorrectDataException("Нет бизнеса с таким id");
        return business;
    }
    private Expert GetExpertById(Guid id)
    {
        var expert = _repository.Get<Expert>(model => model.Id == id).FirstOrDefault();
        if (expert == null)
            throw new IncorrectDataException("Нет эксперта с таким id");
        return expert;
    }
}