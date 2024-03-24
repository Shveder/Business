using Business_monitoring.Data;
using Business_monitoring.DTO;
using Business_monitoring.Models;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business_monitoring.Services;

public class UserService : IUserService
{
    
    private readonly ILogger<UserService> _logger;
    private readonly IDbRepository _repository;

    public UserService(ILogger<UserService> logger, IDbRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    
    public async Task CreateUserAsync(CreateUserRequest request)
    {
        var user = new UserModel(Guid.NewGuid(), request.Login, request.Password, "1323", 1, 0, false, false);
        
        if (request.Password != request.RepeatPassword)
            throw new InvalidDataException("Пароли не совпадают");
        if(await IsLoginUnique(request.Login))
            throw new InvalidDataException("Пользователь с таким логином уже есть в системе");
        if(request.Login.Length is < 4 or > 32)
            throw new InvalidDataException("Длина логина должна быть от 4 до 32 символов");
        if(request.Password.Length is < 4 or > 32)
            throw new InvalidDataException("Длина пароля должна быть от 4 до 32 символов");

        await _repository.Add(user);
        await _repository.SaveChangesAsync();
        _logger.LogInformation($"User created (Login: {request.Login})");
    }

    public async Task<UserModel> Login(LoginRequest request)
    {
        var user = await _repository.Get<UserModel>(model => model.Login == request.Login && model.Password == request.Password).FirstOrDefaultAsync();

        if (user == null)
            throw new InvalidDataException("Неверный логин или пароль");
        
        if (user.IsBlocked)
            throw new AuthorizationException("Аккаунт заблокирован");
        
        if (user.IsDeleted)
            throw new AuthorizationException("Аккаунт удален");

        return user;
    }
    
        

    private async Task<bool> IsLoginUnique(string login)
    {
        var user = await _repository.Get<UserModel>(model => model.Login == login).FirstOrDefaultAsync();
        await _repository.SaveChangesAsync();
        /*var repetitor = repetitorService.getRepetitorByLogin(login);
        var expert = expertService.getExpertByLogin(login);*/
        if (user != null /*|| repetitor != null || expert != null*/)
        {
            return true;
        }
        return false;
    }
    
    
}