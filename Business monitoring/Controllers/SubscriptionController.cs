using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SubscriptionController: ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ILogger<SubscriptionController> logger, ISubscriptionService subscriptionService)
    {
        _logger = logger;
        _subscriptionService = subscriptionService;
    }
    
    /// <summary>
    ///     Add subscription
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Subscription/AddSubscription
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Subscription is added.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpPost("AddSubscription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddSubscription([FromBody] SubscriptionRequest request)
    {
        await _subscriptionService.AddSubscription(request);
        _logger.LogInformation("Подписка добавлена");
        return Ok("Подписка добавлена");
    }
    /// <summary>
    ///     Delete subscription
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Subscription/DeleteSubscription
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Subscription is deleted.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpDelete("DeleteSubscription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DeleteSubscription([FromBody] SubscriptionRequest request)
    {
        await _subscriptionService.DeleteSubscription(request);
        _logger.LogInformation("Подписка удалена");
        return Ok("Подписка удалена");
    }
}