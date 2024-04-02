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
}