using Business_monitoring.Models;

namespace Business_monitoring.Services.Interfaces;

public interface IAdminService
{
    public Task<IQueryable<UserModel>> GetAllUsers();
    public Task<IQueryable<Company>> GetAllCompanies();
    public Task<IQueryable<Expert>> GetAllExperts();
    public Task<IQueryable<LoginHistory>> GetUserLoginHistory(Guid id);
    public Task<IQueryable<Deposits>> GetUserDeposits(Guid id);
    public Task<IQueryable<RecentPasswords>> GetUserPasswords(Guid id);
}