﻿using Business_monitoring.DTO;
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
    
}