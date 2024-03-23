using Business_monitoring.DTO;

namespace Business_monitoring.Services.Interfaces;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserRequest request);
}