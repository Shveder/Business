using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Business_monitoring.Services;

public class AdminService : IAdminService
{
    
    private readonly ILogger<AdminService> _logger;
    private readonly IDbRepository _repository;

    public AdminService(ILogger<AdminService> logger, IDbRepository repository)
    {
        _logger = logger;
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

    public async Task<IQueryable<LoginHistory>> GetUserLoginHistory(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        return _repository.Get<LoginHistory>(model => model.User == user);
    }

    public async Task<IQueryable<Deposits>> GetUserDeposits(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        return _repository.Get<Deposits>(model => model.User == user);
    }
    public async Task<IQueryable<RecentPasswords>> GetUserPasswords(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        return _repository.Get<RecentPasswords>(model => model.User == user);
    }
}