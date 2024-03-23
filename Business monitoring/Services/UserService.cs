using Business_monitoring.Data;
using Business_monitoring.DTO;
using Business_monitoring.Models;
using Business_monitoring.Services.Interfaces;

namespace Business_monitoring.Services;

public class UserService : IUserService
{
    
    private readonly Context _context;
    private readonly ILogger<UserService> _logger;

    public UserService(Context context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    
    public async Task CreateUserAsync(CreateUserRequest request)
    {
        var user = new UserModel(Guid.NewGuid(), request.Login, request.Password, "1323", 1, 0, false, false);
        try
        {
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User created (Login: {request.Login})");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"The user was not created (Login: {request.Login})");
            throw new Exception($"Error creating user: {ex.Message}");
        }
    }
}