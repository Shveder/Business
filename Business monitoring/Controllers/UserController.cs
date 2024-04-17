using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    /// <summary>
    ///     Edit password
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     PUT /User/ChangePassword
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Password changed.</response>
    /// <response code="422">Invalid password data</response>
    [HttpPut("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _userService.ChangePassword(request);
        _logger.LogInformation($"Пользователь с id {request.Id} успешно изменил пароль");
        return Ok($"Пользователь с id {request.Id} успешно изменил пароль");
    }
    
    /// <summary>
    ///     Add credit card
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     POST /User/AddCreditCard
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Card added.</response>
    /// <response code="422">Invalid card data</response>
    [HttpPost("AddCreditCard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddCreditCard([FromBody] AddCardRequest request)
    {
        await _userService.AddCreditCard(request);
        _logger.LogInformation("Карта добавлена");
        return Ok("Карта добавлена");
    }
    
    /// <summary>
    ///     Get User Cards
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     POST /User/GetCards
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of cards.</response>
    [HttpGet("GetCardList")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCardList(Guid id)
    {
        var cardList = await _userService.GetCardList(id);
        _logger.LogInformation("Список карт получен");
        return Ok(cardList);
    }
    
    /// <summary>
    ///     Replenish balance
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     POST /User/ReplenishBalance
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Replenish user balance success.</response>
    [HttpPost("ReplenishBalance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ReplenishBalance([FromBody] DepositRequest request)
    {
        await _userService.ReplenishBalance(request);
        _logger.LogInformation($"Баланс пополнен на {request.Sum}");
        return Ok($"Баланс пополнен на {request.Sum}");
    }
    
    /// <summary>
    ///     Get all businesses
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetBusinesses
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got all businesses.</response>
    [HttpGet("GetBusinesses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinesses()
    {
        _logger.LogInformation("Получен список бизнесов");
        return Ok(await _userService.GetBusinesses());
    }
    
    /// <summary>
    ///     Get all businesses by company
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetBusinessesByCompany
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got all businesses by company.</response>
    [HttpGet("GetBusinessesByCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinessesByCompany(Guid id)
    {
        _logger.LogInformation("Получен список бизнесов");
        return Ok(await _userService.GetBusinessesByCompany(id));
    }
    
    /// <summary>
    ///     Get expert views by business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetExpertViewsByBusiness
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got all expert views by company.</response>
    [HttpGet("GetExpertViewsByBusiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpertViewsByBusiness(Guid id)
    {
        _logger.LogInformation("Получен список мнений");
        return Ok(await _userService.GetExpertViewsByBusiness(id));
    }
    
    /// <summary>
    ///     Get expert views by expert
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetExpertViewsByExpert
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got all expert views by expert.</response>
    [HttpGet("GetExpertViewsByExpert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpertViewsByExpert(Guid id)
    {
        _logger.LogInformation("Получен список мнений");
        return Ok(await _userService.GetExpertViewsByExpert(id));
    }
    
    /// <summary>
    ///     Buy expert view
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /User/BuyExpertView
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Buys expert view for user.</response>
    /// <response code="422">Invalid data.</response>
    [HttpPost("BuyExpertView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> BuyExpertView([FromBody] BuyExpertViewRequest request)
    {
        await _userService.BuyExpertView(request);
        _logger.LogInformation("Доступ к мнениям экспертов получен");
        return Ok("Доступ к мнениям экспертов получен");
    }
    
    /// <summary>
    ///     Is expert view bought
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/IsExpertViewBought
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got status of purchase.</response>
    [HttpGet("IsExpertViewBought")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsExpertViewBought(Guid userId, Guid businessId)
    {
        _logger.LogInformation("Результат покупки получен");
        return Ok(await _userService.GetExpertViewBoughtStatus(userId, businessId));
    }
    
    /// <summary>
    ///     Get recent prices of business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetPricesOfBusinesses
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got recent prices.</response>
    [HttpGet("GetPricesOfBusinesses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPricesOfBusinesses( Guid businessId)
    {
        _logger.LogInformation("Предыдущие цены получены");
        return Ok(await _userService.GetPricesOfBusinesses(businessId));
    }
    
    /// <summary>
    ///     Get notifications
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetNotifications
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got notifications.</response>
    [HttpGet("GetNotifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications(Guid userId)
    {
        _logger.LogInformation("Уведомления получены");
        return Ok(await _userService.GetUserNotifications(userId));
    }
    
    /// <summary>
    ///     Delete notification
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /User/DeleteNotificaton
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Delete notification.</response>
    [HttpDelete("DeleteNotificaton")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotificaton(Guid notifId)
    {
        await _userService.DeleteNotification(notifId);
        _logger.LogInformation("Уведомление удалено");
        return Ok("Уведомление удалено");
    }
    
    /// <summary>
    ///     Buy shares of company
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /User/BuySharesOfCompany
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Shares are bought.</response>
    /// <response code="422">Invalid data or lack of money.</response>
    [HttpPost("BuySharesOfCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> BuySharesOfCompany([FromBody] BuySharesOfCompanyRequest request)
    {
        await _userService.BuySharesOfCompany(request);
        _logger.LogInformation("Акции куплены");
        return Ok("Акции куплены");
    }
    
    /// <summary>
    ///     Get owners by business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetOwnersOfBusiness
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got owners.</response>
    [HttpGet("GetOwnersOfBusiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnersOfBusiness(Guid businessId)
    {
        _logger.LogInformation("Владельцы получены");
        return Ok(await _userService.GetOwnersOfBusiness(businessId));
    }
    
    /// <summary>
    ///     Get ownerships by user
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetOwnershipsByUser
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got ownerships.</response>
    [HttpGet("GetOwnershipsByUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnershipsByUser(Guid userId)
    {
        _logger.LogInformation("Бизнесы получены");
        return Ok(await _userService.GetOwnershipsByUser(userId));
    }
    
    /// <summary>
    ///     Add offer
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /User/AddOffer
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Offer is added.</response>
    [HttpPost("AddOffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddOffer([FromBody] AddOfferRequest request)
    {
        await _userService.AddOffer(request);
        _logger.LogInformation("Предложение добавлено");
        return Ok("Предложение добавлено");
    }
    /// <summary>
    ///     Buy offer
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /User/BuyOffer
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Offer is bought.</response>
    [HttpPost("BuyOffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> BuyOffer([FromBody] BuyOfferRequest request)
    {
        await _userService.BuyOffer(request);
        _logger.LogInformation("Предложение куплено");
        return Ok("Предложение куплено");
    }
    
    /// <summary>
    ///     Delete offer
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /User/DeleteOffer
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Offer is deleted.</response>
    [HttpDelete("DeleteOffer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteOffer(Guid offerId)
    {
        await _userService.DeleteOffer(offerId);
        _logger.LogInformation("Предложение удалено");
        return Ok("Предложение удалено");
    }
    
    /// <summary>
    ///     Get offers
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetAllOffers
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got all offers.</response>
    [HttpGet("GetAllOffers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOffers()
    {
        _logger.LogInformation("Предложения получены");
        return Ok(await _userService.GetAllOffers());
    }
    
    /// <summary>
    ///     Get offers by user
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetOffersByUser
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got offers by user.</response>
    [HttpGet("GetOffersByUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOffersByUser(Guid userId)
    {
        _logger.LogInformation("Предложения получены");
        return Ok(await _userService.GetOffersByUser(userId));
    }
    
    /// <summary>
    ///     Get offers by business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetOffersByBusiness
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got offers by business.</response>
    [HttpGet("GetOffersByBusiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOffersByBusiness(Guid businessId)
    {
        _logger.LogInformation("Предложения получены");
        return Ok(await _userService.GetOffersByBusiness(businessId));
    }
}