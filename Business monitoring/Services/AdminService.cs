using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Business_monitoring.Services;

public class AdminService : IAdminService
{
    
    private readonly IDbRepository _repository;

    public AdminService(IDbRepository repository)
    {
        _repository = repository;
    }

    public Task<IQueryable<UserModel>> GetAllUsers()
    {
        return Task.FromResult(_repository.GetAll<UserModel>());
    }

    public Task<IQueryable<Company>> GetAllCompanies()
    {
        return Task.FromResult(_repository.GetAll<Company>());
    }

    public Task<IQueryable<Expert>> GetAllExperts()
    {
        return Task.FromResult(_repository.GetAll<Expert>());
    }

    public Task<IQueryable<LoginHistory>> GetUserLoginHistory(Guid id)
    {
        var user = GetUserById(id);
        return Task.FromResult(_repository.Get<LoginHistory>(model => model.User == user));
    }

    public Task<IQueryable<Deposits>> GetUserDeposits(Guid id)
    {
        var user = GetUserById(id);
        return Task.FromResult(_repository.Get<Deposits>(model => model.User == user));
    }
    public Task<IQueryable<RecentPasswords>> GetUserPasswords(Guid id)
    {
        var user = GetUserById(id);
        return Task.FromResult(_repository.Get<RecentPasswords>(model => model.User == user));
    }

    public async Task DeleteUser(Guid id)
    {
        await _repository.Delete<UserModel>(id);
        await _repository.SaveChangesAsync();
    }
    
    public async Task DeleteCompany(Guid id)
    {
        await _repository.Delete<Company>(id);
        await _repository.SaveChangesAsync();
    }
    
    public async Task DeleteExpert(Guid id)
    {
        await _repository.Delete<Expert>(id);
        await _repository.SaveChangesAsync();
    }

    public async Task ChangeRole(ChangeRoleRequest request)
    {
        var user = GetUserById(request.Id);
        user.Role = request.Role;
        user.DateUpdated = DateTime.UtcNow;
        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    public async Task SetUserBlockStatus(ChangeStatusRequest request)
    {
        var user = GetUserById(request.Id);
        user.IsBlocked = request.status;
        user.DateUpdated = DateTime.UtcNow;
        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    public async Task SetUserDeleteStatus(ChangeStatusRequest request)
    {
        var user = GetUserById(request.Id);
        user.IsDeleted = request.status;
        user.DateUpdated = DateTime.UtcNow;
        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    public async Task SetCompanyBlockStatus(ChangeStatusRequest request)
    {
        var company = GetCompanyById(request.Id);
        company.IsBlocked = request.status;
        company.DateUpdated = DateTime.UtcNow;
        await _repository.Update(company);
        await _repository.SaveChangesAsync();
    }

    public async Task SetCompanyDeleteStatus(ChangeStatusRequest request)
    {
        var company = GetCompanyById(request.Id);
        company.IsDeleted = request.status;
        company.DateUpdated = DateTime.UtcNow;
        await _repository.Update(company);
        await _repository.SaveChangesAsync();
    }

    public async Task SetExpertBlockStatus(ChangeStatusRequest request)
    {
        var expert = GetExpertById(request.Id);
        expert.IsBlocked = request.status;
        expert.DateUpdated = DateTime.UtcNow;
        await _repository.Update(expert);
        await _repository.SaveChangesAsync();
    }

    public async Task SetExpertDeleteStatus(ChangeStatusRequest request)
    {
        var expert = GetExpertById(request.Id);
        expert.IsDeleted = request.status;
        expert.DateUpdated = DateTime.UtcNow;
        await _repository.Update(expert);
        await _repository.SaveChangesAsync();
    }

    private UserModel GetUserById(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        if (user == null)
            throw new IncorrectDataException("Нет пользователя с таким id");
        return user;
    }
    private Company GetCompanyById(Guid id)
    {
        var company = _repository.Get<Company>(model => model.Id == id).FirstOrDefault();
        if (company == null)
            throw new IncorrectDataException("Нет компании с таким id");
        return company;
    }
    private Expert GetExpertById(Guid id)
    {
        var expert = _repository.Get<Expert>(model => model.Id == id).FirstOrDefault();
        if (expert == null)
            throw new IncorrectDataException("Нет эксперта с таким id");
        return expert;
    }
    private Business GetBusinessByBusinessId(Guid id)
    {
        var business = _repository.Get<Business>(model => model.Id == id).Include(business1 => business1.Company).FirstOrDefault();
        if (business == null)
            throw new IncorrectDataException("Нет бизнеса с таким id");
        return business;
    }

    public async Task<Business> GetBusinessById(Guid id)
    {
        return GetBusinessByBusinessId(id);
    }
}