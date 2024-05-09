using Business_monitoring.DTO;
using Business_monitoring.Models;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Services.Interfaces;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserRequest request);
    public Task<IModels> Login(LoginRequest request);
    public Task ChangePassword(ChangePasswordRequest request);
    public Task AddCreditCard(AddCardRequest request);
    public Task<IQueryable<Card>>GetCardList(Guid id);
    public Task ReplenishBalance(DepositRequest request);
    public Task<IQueryable<Business>> GetBusinesses();
    public Task<IQueryable<Business>> GetBusinessesByCompany(Guid id);
    public Task<IQueryable<ExpertView>> GetExpertViewsByBusiness(Guid id);
    public Task<IQueryable<ExpertView>> GetExpertViewsByExpert(Guid id);
    public Task<IQueryable<PricesOfShares>> GetPricesOfShares(Guid businessId);
    public Task BuyExpertView(BuyExpertViewRequest request);
    public Task<bool> GetExpertViewBoughtStatus(Guid userId, Guid businessId);
    public Task<IQueryable<RecentPricesOfBusiness>> GetPricesOfBusinesses(Guid id);
    public Task<IQueryable<Notification>> GetUserNotifications(Guid userId);
    public Task DeleteNotification(Guid id);
    public Task BuySharesOfCompany(BuySharesOfCompanyRequest request);
    public Task<IQueryable<Owners>> GetOwnersOfBusiness(Guid businessId);
    public Task<IQueryable<Owners>> GetOwnershipsByUser(Guid userId);
    public Task AddOffer(AddOfferRequest request);
    public Task BuyOffer(BuyOfferRequest request);
    public Task DeleteOffer(Guid offerId);
    public Task<IQueryable<Offer>> GetAllOffers();
    public Task<IQueryable<Offer>> GetOffersByBusiness(Guid businessId);
    public Task<IQueryable<Offer>> GetOffersByUser(Guid userId);
    public int GetCountOfNotifications(Guid userId);
    public Task ChangeLogin(ChangeLoginRequest request);
}