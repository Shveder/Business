using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;

public class ExpertController : ControllerBase
{
    private readonly ILogger<ExpertController> _logger;
    private readonly IExpertService _expertService;

    public ExpertController(ILogger<ExpertController> logger, IExpertService expertService)
    {
        _logger = logger;
        _expertService = expertService;
    }
    
    /// <summary>
    ///     Add one expert view
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Expert/AddExpertView
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">View is added.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpPost("AddExpertView")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddExpertView([FromBody] AddViewRequest request)
    {
        await _expertService.AddExpertView(request);
        _logger.LogInformation("Мнение добавлено");
        return Ok("Мнение добавлено");
    }

}