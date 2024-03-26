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
}