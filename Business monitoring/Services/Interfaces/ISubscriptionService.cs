using Business_monitoring.DTO;
using Business_monitoring.Models;

namespace Business_monitoring.Services.Interfaces;

public interface ISubscriptionService
{
    public Task AddSubscription(SubscriptionRequest request);
    public Task DeleteSubscription(SubscriptionRequest request);
    public Task Notify(Guid userId, String text);
    public Task<IEnumerable<Subscription>> GetAllSubscribers(Guid businessId);
}