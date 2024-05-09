using Business_monitoring.DTO;
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
    public Task<IQueryable<PurchaceOfView>>GetUserPurchases(Guid id);
    public Task<Business> GetBusinessById(Guid id);
    public Task DeleteUser(Guid id);
    public Task DeleteCompany(Guid id);
    public Task DeleteExpert(Guid id);
    public Task DeleteBusiness(Guid id);
    public Task ChangeRole(ChangeRoleRequest request);
    public Task SetUserBlockStatus(ChangeStatusRequest request);
    public Task SetUserDeleteStatus(ChangeStatusRequest request);
    public Task SetCompanyBlockStatus(ChangeStatusRequest request);
    public Task SetCompanyDeleteStatus(ChangeStatusRequest request);
    public Task SetExpertBlockStatus(ChangeStatusRequest request);
    public Task SetExpertDeleteStatus(ChangeStatusRequest request);
    
}