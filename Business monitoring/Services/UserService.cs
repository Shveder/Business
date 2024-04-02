using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Models.Interfaces;
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
        
        if (request.Password != request.RepeatPassword)
            throw new IncorrectDataException("Пароли не совпадают");
        if(await IsLoginUnique(request.Login))
            throw new IncorrectDataException("Пользователь с таким логином уже есть в системе");
        if(request.Login.Length is < 4 or > 32)
            throw new IncorrectDataException("Длина логина должна быть от 4 до 32 символов");
        if(request.Password.Length is < 4 or > 32)
            throw new IncorrectDataException("Длина пароля должна быть от 4 до 32 символов");

        if (request.Role == 1)
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Password = request.Password,
                Salt = "123",
                Role = 1,
                Balance = 0,
                IsDeleted = false,
                IsBlocked = false
            };
            await _repository.Add(user);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"User created (Login: {request.Login})");
        }
        if (request.Role == 2)
        {
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Password = request.Password,
                Salt = "123",
                Email = request.Email,
                Phone = request.Phone,
                IsDeleted = false,
                IsBlocked = false
            };
            if (!IsValidPhoneNumber(request.Phone))
                throw new IncorrectDataException("Неверный формат номера телефона");
            if (!IsValidEmail(request.Email))
                throw new IncorrectDataException("Неверный формат email");
            await _repository.Add(company);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"Company created (Login: {request.Login})");
        }
        
        if (request.Role == 3)
        {
            var expert = new Expert()
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Password = request.Password,
                Salt = "123",
                Level = 1,
                IsDeleted = false,
                IsBlocked = false
            };
            await _repository.Add(expert);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"Expert created (Login: {request.Login})");
        }
    }

    public async Task<IModels> Login(LoginRequest request)
    {
        var user = await _repository
            .Get<UserModel>(model => model.Login == request.Login && model.Password == request.Password)
            .FirstOrDefaultAsync();
        var company = await _repository
            .Get<Company>(model => model.Login == request.Login && model.Password == request.Password)
            .FirstOrDefaultAsync();
        var expert = await _repository
            .Get<Expert>(model => model.Login == request.Login && model.Password == request.Password)
            .FirstOrDefaultAsync();

        if (user == null && company == null && expert == null)
            throw new IncorrectDataException("Неверный логин или пароль");

        if ((user != null && user.IsBlocked) || (company != null && company.IsBlocked) ||
            (expert != null && expert.IsBlocked))
            throw new AuthorizationException("Аккаунт заблокирован");

        if ((user != null && user.IsDeleted) || (company != null && company.IsDeleted) ||
            (expert != null && expert.IsDeleted))
            throw new AuthorizationException("Аккаунт удален");

        if (user != null)
        {
            await AddLoginHistory(user);
            return user;
        }
        if (company != null)
            return company;
        if (expert != null)
            return expert;

        return null!;
    }
    private async Task AddLoginHistory(UserModel user)
    {
        
        var loginHistory = new LoginHistory
        {
            Ip = GetLocalIPv4Address(),
            User = user
        };

        await _repository.Add(loginHistory);
        await _repository.SaveChangesAsync();
    }
    static string GetLocalIPv4Address()
    {
        string localIPv4Address = null;

        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIPv4Address = ipAddress.Address.ToString();
                        break;
                    }
                }
            }

            if (localIPv4Address != null)
            {
                break;
            }
        }

        return localIPv4Address;
    }

    public async Task ChangePassword(ChangePasswordRequest request)
    {
        var user = await _repository.Get<UserModel>(model => model.Id == request.Id).FirstOrDefaultAsync();
        if (!(request.PreviousPassword == user.Password))
            throw new IncorrectDataException("Пароль неверный!");
        if (request.NewPassword.Length is < 4 or > 32)
            throw new IncorrectDataException("Пароль должен быть больше 4 и меньше 32 символов!");
        
        user.Password = request.NewPassword;
        user.DateUpdated = DateTime.UtcNow;
        
        await _repository.Update(user);
        await _repository.SaveChangesAsync();

        await AddRecentPassword(user, request.PreviousPassword);
    }

    private async Task AddRecentPassword(UserModel user, string oldPassword)
    {
        var recentPassword = new RecentPasswords
        {
            Id = Guid.NewGuid(),
            Password = oldPassword,
            User = user
        };
        await _repository.Add(recentPassword);
        await _repository.SaveChangesAsync();
    }
    private bool IsValidEmail(string email)
    {
        string emailRegex = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
        Regex regex = new Regex(emailRegex);
        Match match = regex.Match(email);
        return match.Success;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        string phoneRegex = @"^[\d+]{10,}$";
        Regex regex = new Regex(phoneRegex);
        Match match = regex.Match(phoneNumber);
        return match.Success;
    }
    private async Task<bool> IsLoginUnique(string login)
    {
        var user = await _repository.Get<UserModel>(model => model.Login == login).FirstOrDefaultAsync();
        var company = await _repository.Get<Company>(model => model.Login == login).FirstOrDefaultAsync();
        var expert = await _repository.Get<Expert>(model => model.Login == login).FirstOrDefaultAsync();
        
        await _repository.SaveChangesAsync();
        if (user != null || company != null || expert != null)
        {
            return true;
        }
        return false;
    }

    public async Task AddCreditCard(AddCardRequest request)
    {
        if (!IsValidCreditCardNumber(request.Number))
            throw new IncorrectDataException("Неверный номер карты");
        if (!IsValidCardholderName(request.Name))
            throw new IncorrectDataException("Неверное имя держателя");
        if (!IsValidExpirationDate(request.Date))
            throw new IncorrectDataException("Неверная дата действия");
        if (!IsValidCvv(request.Cvv))
            throw new IncorrectDataException("Неверный cvv");
        
        var user = await _repository.Get<UserModel>(model => model.Id == request.Id).FirstOrDefaultAsync();

        var card = new Card()
        {
            CardNumber = request.Number,
            Name = request.Name,
            Date = request.Date,
            Cvv = request.Cvv,
            User = user
        };
        await _repository.Add(card);
        await _repository.SaveChangesAsync();

    }

    public async Task<IQueryable<Card>>GetCardList(Guid id)
    {
        var user = await _repository.Get<UserModel>(model => model.Id == id).FirstOrDefaultAsync();
        var list = _repository.Get<Card>(card => card.User == user);
        return list;
    }

    private static bool IsValidCreditCardNumber(string creditCardNumber)
    {
        creditCardNumber = creditCardNumber.Replace(" ", "");
        if (creditCardNumber.Length < 13 || creditCardNumber.Length > 19)
        {
            return false;
        }
        
        if (!creditCardNumber.All(char.IsDigit))
        {
            return false;
        }
        
        int sum = 0;
        bool alternate = false;
        for (int i = creditCardNumber.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(creditCardNumber[i].ToString());
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }
            sum += digit;
            alternate = !alternate;
        }
        return (sum % 10 == 0);
    }

    private static bool IsValidCardholderName(string name)
    {

        return Regex.IsMatch(name, "^[a-zA-Z ]+$") && name.Length <= 26;
    }

    private static bool IsValidExpirationDate(string date)
    {

        if (!Regex.IsMatch(date, @"^\d{2}/\d{2}$"))
        {
            return false;
        }


        string[] parts = date.Split('/');
        int month, year;
        if (!int.TryParse(parts[0], out month) || !int.TryParse(parts[1], out year))
        {
            return false;
        }


        int currentYear = DateTime.Now.Year % 100;
        return month >= 1 && month <= 12 && year >= currentYear;
    }

    private static bool IsValidCvv(string cvv)
    {

        return Regex.IsMatch(cvv, @"^\d{3,4}$");
    }

    public async Task ReplenishBalance(DepositRequest request)
    {
        var user = await _repository.Get<UserModel>(model => model.Id == request.Id).FirstOrDefaultAsync();

        if (user != null)
        {
            var cardList = await GetCardList(user.Id);

            if (!cardList.Any())
                throw new IncorrectDataException("Сначала добавьте карту");
        

            user.Balance += request.Sum;
            await _repository.Update(user);
            
            var deposit = new Deposits()
            {
                SumOfDeposit = request.Sum,
                User = user
            };
            await _repository.Add(deposit);
            
            await _repository.SaveChangesAsync();
        }
    }
    public async Task<IQueryable<Business>> GetBusinesses()
    {
        return await Task.FromResult(_repository.GetAll<Business>().Include(b => b.Company));
    }
}