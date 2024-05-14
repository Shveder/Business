using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Business_monitoring.DTO;
using Business_monitoring.Exceptions;
using Business_monitoring.Models;
using Business_monitoring.Models.Interfaces;
using Business_monitoring.Repository.Interfaces;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Business_monitoring.Services;

public class UserService : IUserService
{
    
    private readonly ILogger<UserService> _logger;
    private readonly IDbRepository _repository;
    private readonly ISubscriptionService _subscriptionService;

    public UserService(ILogger<UserService> logger, IDbRepository repository, ISubscriptionService subscriptionService)
    {
        _logger = logger;
        _repository = repository;
        _subscriptionService = subscriptionService;
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
        
        
        request.Password = Hash(request.Password);
        string salt = GetSalt();
        request.Password = Hash(request.Password + salt);
        
        switch (request.Role)
        {
            case 1:
                var user = new UserModel
                {
                    Id = Guid.NewGuid(),
                    Login = request.Login,
                    Password = request.Password,
                    Salt = salt,
                    Role = 1,
                    Balance = 0,
                    IsDeleted = false,
                    IsBlocked = false
                };
                await _repository.Add(user);
                await _repository.SaveChangesAsync();
                _logger.LogInformation($"User created (Login: {request.Login})");
            break;
            case 2:
                
                    var company = new Company()
                    {
                        Id = Guid.NewGuid(),
                        Login = request.Login,
                        Password = request.Password,
                        Salt = salt,
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
                break;

                case 3:
                
                    var expert = new Expert()
                    {
                        Id = Guid.NewGuid(),
                        Login = request.Login,
                        Password = request.Password,
                        Salt = salt,
                        Level = 1,
                        IsDeleted = false,
                        IsBlocked = false
                    };
                    await _repository.Add(expert);
                    await _repository.SaveChangesAsync();
                    _logger.LogInformation($"Expert created (Login: {request.Login})");
                break;
        }
    }
    private string GetSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    private string Hash(string inputString)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
        public async Task<IModels> Login(LoginRequest request)
    {
        var user1 = await _repository
            .Get<UserModel>(model => model.Login == request.Login)
            .FirstOrDefaultAsync();
        var company1 = await _repository
            .Get<Company>(model => model.Login == request.Login)
            .FirstOrDefaultAsync();
        var expert1 = await _repository
            .Get<Expert>(model => model.Login == request.Login)
            .FirstOrDefaultAsync();
        
        if (user1 == null && company1 == null && expert1 == null)
            throw new IncorrectDataException("Неверный логин или пароль");
        

        if (user1 != null)
        {
            var trueUser = user1;
            request.Password = Hash(request.Password);
            request.Password = Hash(request.Password + trueUser.Salt);
        }

        if (company1 != null)
        {
            var trueUser = company1;
            request.Password = Hash(request.Password);
            request.Password = Hash(request.Password + trueUser.Salt);
        }
        if (expert1 != null)
        {
            var trueUser = expert1;
            request.Password = Hash(request.Password);
            request.Password = Hash(request.Password + trueUser.Salt);
        }
        
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
        
        return expert ?? null!;
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

        string prevPassword = request.PreviousPassword; 
        prevPassword = Hash(prevPassword);
        prevPassword = Hash(prevPassword + user?.Salt);
        
        if (!(prevPassword == user.Password))
            throw new IncorrectDataException("Пароль неверный!");
        if (prevPassword == request.NewPassword)
            throw new IncorrectDataException("Новый пароль должен отличаться от старого");
        if (request.NewPassword.Length is < 4 or > 32)
            throw new IncorrectDataException("Пароль должен быть больше 4 и меньше 32 символов!");
        
        request.NewPassword = Hash(request.NewPassword);
        request.NewPassword = Hash(request.NewPassword + user?.Salt);
        
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

    public async Task<IQueryable<PricesOfShares>> GetPricesOfShares(Guid businessId)
    {
        var business = GetBusinessById(businessId);
        return await Task.FromResult(_repository.Get<PricesOfShares>(shares =>shares.Business == business));
    }
    private Company GetCompanyById(Guid id)
    {
        var company = _repository.Get<Company>(model => model.Id == id).FirstOrDefault();
        if (company == null)
            throw new IncorrectDataException("Нет компании с таким id");
        return company;
    }
    public async Task<IQueryable<Business>> GetBusinessesByCompany(Guid id)
    {
        var company = GetCompanyById(id);
        return await Task.FromResult(_repository.Get<Business>(business => business.Company == company).Include(b => b.Company));
    }
    private Business GetBusinessById(Guid id)
    {
        var business = _repository.Get<Business>(model => model.Id == id).FirstOrDefault();
        if (business == null)
            throw new IncorrectDataException("Нет бизнеса с таким id");
        return business;
    }
    public async Task<IQueryable<ExpertView>> GetExpertViewsByBusiness(Guid id)
    {
        var business = GetBusinessById(id);
        return await Task.FromResult(_repository.Get<ExpertView>(view =>  view.Business == business).Include(b => b.Business).Include(b => b.Expert));
    }
    private Expert GetExpertById(Guid id)
    {
        var expert = _repository.Get<Expert>(model => model.Id == id).FirstOrDefault();
        if (expert == null)
            throw new IncorrectDataException("Нет эксперта с таким id");
        return expert;
    }
    public async Task<IQueryable<ExpertView>> GetExpertViewsByExpert(Guid id)
    {
        var expert = GetExpertById(id);
        return await Task.FromResult(_repository.Get<ExpertView>(view =>  view.Expert == expert).Include(b => b.Business).Include(b=>b.Business.Company));
    }

    public async Task BuyExpertView(BuyExpertViewRequest request)
    {
        if (IsExpertViewBought(request))
            throw new IncorrectDataException("Доступ к мнениям уже куплен");
        var business = GetBusinessById(request.BusinessId);
        var user = GetUserById(request.UserId);
        if(user.Balance < business.ExpertViewPrice)
            throw new IncorrectDataException("На балансе пользоавтеля недостаточно средств");

        var purchase = new PurchaceOfView()
        {
            User = user,
            Business = business,
            Price = business.ExpertViewPrice
        };
        user.Balance -= business.ExpertViewPrice;
        user.DateUpdated = DateTime.UtcNow;

        await _repository.Update(user);
        await _repository.Add(purchase);
        await _repository.SaveChangesAsync();
    }
    private UserModel GetUserById(Guid id)
    {
        var user = _repository.Get<UserModel>(model => model.Id == id).FirstOrDefault();
        if (user == null)
            throw new IncorrectDataException("Нет пользователя с таким id");
        return user;
    }
    private bool IsExpertViewBought(BuyExpertViewRequest request)
    {
        var business = GetBusinessById(request.BusinessId);
        var user = GetUserById(request.UserId);
        var purchase = _repository.Get<PurchaceOfView>(p => p.Business == business && p.User == user).FirstOrDefault();
        return purchase != null;
    }

    public Task<bool> GetExpertViewBoughtStatus(Guid userId, Guid businessId)
    {
        return Task.FromResult(IsExpertViewBought(new BuyExpertViewRequest(userId,businessId)));
    }
    
    public Task<IQueryable<RecentPricesOfBusiness>> GetPricesOfBusinesses(Guid id)
    {
        var business = GetBusinessById(id);
        return Task.FromResult(_repository.Get<RecentPricesOfBusiness>(model => model.Business == business));
    }

    public Task<IQueryable<Notification>> GetUserNotifications(Guid userId)
    {
        var user = GetUserById(userId);
        return Task.FromResult(_repository.Get<Notification>(notification => notification.User == user));
    }
    public int GetCountOfNotifications(Guid userId)
    {
        var user = GetUserById(userId);
        return _repository.Get<Notification>(notification => notification.User == user).Count();
    }

    public async Task DeleteNotification(Guid id)
    {
        await _repository.Delete<Notification>(id);
        await _repository.SaveChangesAsync();
    }

    public async Task BuySharesOfCompany(BuySharesOfCompanyRequest request)
    {
        var user = GetUserById(request.UserId);
        var business = GetBusinessById(request.BusinessId);
        if (request.NumberOfShares > business.NumberToSell || request.NumberOfShares < 1)
            throw new IncorrectDataException("Неверное значение числа акций");
        
        double fullPrice = request.NumberOfShares * business.PriceOfShare;
        
        if (fullPrice > user.Balance)
            throw new IncorrectDataException("Недостаточно средств для покупки");
        var owner = GetOwnershipById(request.UserId, request.BusinessId);
        
        if (owner != null)
        {
            user.Balance -= fullPrice;
            user.DateUpdated = DateTime.UtcNow;
            owner.NumberOfShares += request.NumberOfShares;
            owner.DateUpdated = DateTime.UtcNow;
            business.NumberToSell -= request.NumberOfShares;

            await _repository.Update(business);
            await _repository.Update(user);
            await _repository.Update(owner);
            await _repository.SaveChangesAsync();

            await NotifySubs($"Пользователь {user.Login}" +
                             $" докупил {request.NumberOfShares} акций компании {business.Name}", business.Id);

            return;  
        }

        var newOwner = new Owners()
        {
            User = user,
            Business = business,
            NumberOfShares = request.NumberOfShares
        };
        user.Balance -= fullPrice;
        user.DateUpdated = DateTime.UtcNow;

        NotifySubs($"Пользователь {user.Login}" +
                   $" купил {request.NumberOfShares} акций компании {business.Name}", business.Id);
        
        business.NumberToSell -= request.NumberOfShares;

        await _repository.Update(business);
        await _repository.Update(user);
        await _repository.Add(newOwner);
        await _repository.SaveChangesAsync();

    }
    
    private async Task NotifySubs(string text, Guid businessId)
    {
        var subs = await _subscriptionService.GetAllSubscribers(businessId);

        foreach (var sub in subs)
        {
            await _subscriptionService.Notify(sub.User.Id, text);
        }
    }
    
    private Owners? GetOwnershipById(Guid userId, Guid businessId)
    {
        var ownership = _repository.Get<Owners>(model => model.User.Id == userId && model.Business.Id == businessId).
            Include(b => b.Business).Include(b=>b.User).FirstOrDefault();
        return ownership;
    }

    public async Task<IQueryable<Owners>> GetOwnersOfBusiness(Guid businessId)
    {
        var business = GetBusinessById(businessId);
        return await Task.FromResult(_repository
                .Get<Owners>(model => model.Business == business)
                .Include(o => o.User) 
        );
    }

    public async Task<IQueryable<Owners>> GetOwnershipsByUser(Guid userId)
    {
        var user = GetUserById(userId);
        return await Task.FromResult(_repository
            .Get<Owners>(model => model.User == user)
            .Include(o => o.Business)
        );
    }

    public async Task AddOffer(AddOfferRequest request)
    {
        var business = GetBusinessById( request.BusinessId);
        var user = GetUserById(request.UserId);
        var ownership = GetOwnershipById(request.UserId, request.BusinessId);
        int numberInOffers = GetNumberOfSharesOfUserToSell(request.UserId, request.BusinessId);
        
        if (ownership == null)
            throw new IncorrectDataException("Пользователь не владеет этими акциями");
        if (request.NumberOfShares < 1)
            throw new IncorrectDataException("Число акций на продажу должно быть больше 0");
        if (ownership.NumberOfShares - numberInOffers < request.NumberOfShares)
            throw new IncorrectDataException("Недостаточно акций для продажи");
        if (request.PriceOfShare < 0)
            throw new IncorrectDataException("Неверная цена акции");
        
        var newOffer = new Offer()
        {
            User = user,
            Business = business,
            NumberOfShares = request.NumberOfShares,
            PriceForShare = request.PriceOfShare
        };
        await _repository.Add(newOffer);
        await _repository.SaveChangesAsync();
    }
    public async Task DeleteOffer(Guid offerId)
    {
        await _repository.Delete<Offer>(offerId);
        await _repository.SaveChangesAsync();
    }
    private Offer GetOfferById(Guid id)
    {
        var offer = _repository.Get<Offer>(model => model.Id == id)
            .Include(o=> o.Business)
            .Include(o=> o.User).FirstOrDefault();
        if (offer == null)
            throw new IncorrectDataException("Нет предложения с таким id");
        return offer;
    }
    
    private int GetNumberOfSharesOfUserToSell(Guid userId, Guid businessId)
    {
        int number = 0;
        var offers = _repository.Get<Offer>()
            .Include(o => o.User).Include(o => o.Business);
        foreach (var offer in offers)
        {
            number += offer.NumberOfShares;
        }
        return number;
    }
    public async Task BuyOffer(BuyOfferRequest request)
    {
        var user = GetUserById(request.UserId);
        var offer = GetOfferById(request.OfferId);
        var business = GetBusinessById(offer.Business.Id);
        
        double fullPrice = offer.NumberOfShares * offer.PriceForShare;
        if (user.Balance < fullPrice)
            throw new IncorrectDataException("На балансе недостаточно средств для покупки предложения");
        
        var owner = GetOwnershipById(user.Id, offer.Business.Id);
        var oldOwner = GetOwnershipById(offer.User.Id, offer.Business.Id);
        
        if (owner != null)
        {
            user.Balance -= fullPrice;
            user.DateUpdated = DateTime.UtcNow;
            owner.NumberOfShares += offer.NumberOfShares;
            owner.DateUpdated = DateTime.UtcNow;

            if (oldOwner != null)
            {
                oldOwner.NumberOfShares -= offer.NumberOfShares;
                oldOwner.DateUpdated = DateTime.UtcNow;
                
                await _repository.Update(oldOwner);
                if (oldOwner.NumberOfShares == 0)
                {
                    await _repository.Delete<Owners>(oldOwner.Id);
               
                }
            }

            if (offer.PriceForShare > business.PriceOfShare)
            {
                business.PriceOfShare += offer.PriceForShare * offer.NumberOfShares / 500;
            }
            else
            {
                business.PriceOfShare -= offer.PriceForShare * offer.NumberOfShares / 500;
            }

            offer.User.Balance += fullPrice;
            
            await NotifySubs($"Пользователь {user.Login}" +
                       $" купил {offer.NumberOfShares} акций компании {offer.Business.Name} у пользователя {offer.User.Login}." +
                       $"В связи с этим цена акции изменилась и составила {business.PriceOfShare}"
                , offer.Business.Id);

            var priceOfShare = new PricesOfShares()
            {
                Business = business,
                PriceOfShare = business.PriceOfShare
            };
            await _repository.Add(priceOfShare);
            await _repository.Update(offer.User);
            await _repository.Update(business);
            await _repository.Update(user);
            await _repository.Update(owner);
            await _repository.Delete<Offer>(offer.Id);
            await _repository.SaveChangesAsync();
            return;  
        }

        
        var newOwner = new Owners()
        {
            NumberOfShares = offer.NumberOfShares,
            Business = business,
            User = user
        };
        user.Balance -= fullPrice;
        user.DateUpdated = DateTime.UtcNow;
        
        if (oldOwner != null)
        {
            oldOwner.NumberOfShares -= offer.NumberOfShares;
            oldOwner.DateUpdated = DateTime.UtcNow;
            await _repository.Update(oldOwner);
            if (oldOwner.NumberOfShares == 0)
            {
                await _repository.Delete<Owners>(oldOwner.Id);
               
            }
        }

        if (offer.PriceForShare > business.PriceOfShare)
        {
            business.PriceOfShare += offer.PriceForShare / 10;
        }
        else
        {
            business.PriceOfShare -= (offer.PriceForShare / 10);
        }
        offer.User.Balance += fullPrice;
        
        await NotifySubs($"Пользователь {user.Login}" +
                   $" купил {offer.NumberOfShares} акций компании {offer.Business.Name} у пользователя {offer.User.Login}." +
                   $"В связи с этим цена акции изменилась и составила {business.PriceOfShare}"
            , offer.Business.Id);
        
        var priceOfShare1 = new PricesOfShares()
        {
            Business = business,
            PriceOfShare = business.PriceOfShare
        };
        await _repository.Add(priceOfShare1);
        await _repository.Update(offer.User);
        await _repository.Update(business);
        await _repository.Update(user);
        await _repository.Add(newOwner);
        await _repository.Delete<Offer>(offer.Id);
        await _repository.SaveChangesAsync();
    }
    public async Task<IQueryable<Offer>> GetAllOffers()
    {
        return await Task.FromResult(_repository.Get<Offer>()
            .Include(o=>o.User).Include(o=>o.Business));
    }

    public async Task<IQueryable<Offer>> GetOffersByBusiness(Guid businessId)
    {
        var business = GetBusinessById(businessId);
        return await Task.FromResult(_repository.Get<Offer>(offer => offer.Business == business)
            .Include(offer=>offer.User));
    }

    public async Task<IQueryable<Offer>> GetOffersByUser(Guid userId)
    {
        var user = GetUserById(userId);
        return await Task.FromResult(_repository.Get<Offer>(offer => offer.User == user)
            .Include(offer=>offer.Business));
    }

    public async Task ChangeLogin(ChangeLoginRequest request)
    {
        var user = GetUserById(request.Id);
        if (request.NewLogin == user.Login)
            throw new IncorrectDataException("Логин должен отличаться от старого");
        if(await IsLoginUnique(request.NewLogin))
            throw new IncorrectDataException("Пользователь с таким логином уже есть в системе");
        
        user.Login = request.NewLogin;
        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }
}