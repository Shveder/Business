using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;

public class CompanyController : ControllerBase
{
    private readonly ILogger<CompanyController> _logger;
    private readonly ICompanyService _companyService;

    public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService)
    {
        _logger = logger;
        _companyService = companyService;
    }
    
    /// <summary>
    ///     Add one business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Company/AddBusiness
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Business is added.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpPost("AddBusiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddBusiness([FromBody] AddBusinessRequest request)
    {
        await _companyService.AddBusiness(request);
        _logger.LogInformation("Бизнес добавлен");
        return Ok("Бизнес добавлен");
    }
    
    /// <summary>
    ///     Change price of business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Put /Company/ChangeBusinessPrice
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Price is changed.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpPut("ChangeBusinessPrice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeBusinessPrice([FromBody] ChangeBusinessPriceRequest request)
    {
        await _companyService.ChangeBusinessPrice(request);
        _logger.LogInformation("Цена бизнеса измененена");
        return Ok($"Цена бизнеса измененена на {request.NewPrice}");
    }
    
    /// <summary>
    ///     Add gain of business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Post /Company/AddGainOfCompany
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Gain is added.</response>
    /// <response code="422">Invalid input data.</response>
    [HttpPost("AddGainOfCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddGainOfCompany([FromBody] AddGainRequest request)
    {
        await _companyService.AddGainOfCompany(request);
        _logger.LogInformation("Размер прибыли добавлен");
        return Ok("Размер прибыли добавлен");
    }
      
    /// <summary>
    ///     Get recent gains of business
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /Company/GetGainsOfBusinesses
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got recent gains.</response>
    [HttpGet("GetGainsOfBusinesses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGainsOfBusinesses( Guid businessId)
    {
        _logger.LogInformation("Прибыль прошлых лет получена");
        return Ok(await _companyService.GetGainsOfBusinesses(businessId));
    }

    /// <summary>
    ///     Set number of shares to sell
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /Company/SetNumberOfSharesToSell
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Number is set.</response>
    /// <response code="422">Invalid number of shares.</response> 
    
    [HttpPut("SetNumberOfSharesToSell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetNumberOfSharesToSell([FromBody] SetNumberOfSharesRequest request)
    {
        _logger.LogInformation("Количество акций установлено");
        await _companyService.SetNumberOfSharesToSell(request);
        return Ok("Количество акций установлено");
    }
}