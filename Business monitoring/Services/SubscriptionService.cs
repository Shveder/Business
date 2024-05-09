using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business_monitoring.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IDbRepository _repository;

    public SubscriptionService(IDbRepository repository)
    {
        _repository = repository;
    }
    public async Task AddSubscription(SubscriptionRequest request)
    {
        if (GetIsSubscribed(request))
            throw new IncorrectDataException("Вы уже подписаны");
        
        var user = GetUserById(request.UserId);
        var business = GetBusinessById(request.BusinessId);

        var subscription = new Subscription()
        {
            User = user,
            Business = business
        };
        await _repository.Add(subscription);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteSubscription(SubscriptionRequest request)
    {
        if (!GetIsSubscribed(request))
            throw new IncorrectDataException("Вы не подписаны");
        
        var user = GetUserById(request.UserId);
        var business = GetBusinessById(request.BusinessId);

        var subscription = _repository.Get<Subscription>(subscription =>
            subscription.Business == business
            && subscription.User == user).FirstOrDefault();

        await _repository.Delete<Subscription>(subscription!.Id);
        await _repository.SaveChangesAsync();
    }

    public async Task Notify(Guid userId, string text)
    {
        var user = GetUserById(userId);
        var notification = new Notification()
        {
            User = user,
            Text = text
        };
        await _repository.Add(notification);
        await _repository.SaveChangesAsync();
    }
    public bool GetIsSubscribed(SubscriptionRequest request)
    {
        var user = GetUserById(request.UserId);
        var business = GetBusinessById(request.BusinessId);
    
        var subscription = _repository.Get<Subscription>(subscription =>
            subscription.Business == business
            && subscription.User == user).FirstOrDefault();

        // Если подписка найдена, возвращаем true, иначе false
        return subscription != null;
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscribers(Guid businessId)
    {
        var business = GetBusinessById(businessId);
        var subscriptions = await _repository.Get<Subscription>(s =>
            s.Business == business).Include(s => s.User).ToListAsync();
        return subscriptions;
    }
    
    private UserModel GetUserById(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        if (user == null)
            throw new IncorrectDataException("Нет пользователя с таким id");
        return user;
    }
    private Business GetBusinessById(Guid id)
    {
        var business = _repository.Get<Business>(model => model.Id == id).FirstOrDefault();
        if (business == null)
            throw new IncorrectDataException("Нет бизнеса с таким id");
        return business;
    }
}