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

}