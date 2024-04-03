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
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
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
    
}