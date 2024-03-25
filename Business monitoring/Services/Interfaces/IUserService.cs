using Business_monitoring.DTO;
using Business_monitoring.Models;

namespace Business_monitoring.Services.Interfaces;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserRequest request);
    public Task<UserModel> Login(LoginRequest request);
    public Task ChangePassword(ChangePasswordRequest request);
}