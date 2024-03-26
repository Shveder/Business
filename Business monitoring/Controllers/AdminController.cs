using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IAdminService _adminService;

    public AdminController(ILogger<AdminController> logger, IAdminService adminService)
    {
        _logger = logger;
        _adminService = adminService;
    }
    /// <summary>
    ///     Get all users
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetAllUsers
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of users.</response>
    [HttpGet("GetAllUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Список пользователей получен");
        return Ok(await _adminService.GetAllUsers());
    }
    
    /// <summary>
    ///     Get all companies
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetAllCompanies
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of companies.</response>
    [HttpGet("GetAllCompanies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCompanies()
    {
        _logger.LogInformation("Список компаний получен");
        return Ok(await _adminService.GetAllCompanies());
    }
    
    /// <summary>
    ///     Get all experts
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetAllExperts
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of experts.</response>
    [HttpGet("GetAllExperts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllExperts()
    {
        _logger.LogInformation("Список экспертов получен");
        return Ok(await _adminService.GetAllExperts());
    }
    
    /// <summary>
    ///     Get user deposits
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetUserDeposits
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of user deposits.</response>
    [HttpGet("GetUserDeposits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserDeposits(Guid id)
    {
        _logger.LogInformation("Список пополнений получен");
        return Ok(await _adminService.GetUserDeposits(id));
    }
    
    /// <summary>
    ///     Get user login history
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetUserLoginHistory
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of user login history.</response>
    [HttpGet("GetUserLoginHistory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserLoginHistory(Guid id)
    {
        _logger.LogInformation("Список историй входов получен");
        return Ok(await _adminService.GetUserLoginHistory(id));
    }
    /// <summary>
    ///     Get user recent passwords
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Get /User/GetUserRecentPasswords
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Got list of user recent passwords.</response>
    [HttpGet("GetUserRecentPasswords")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRecentPasswords(Guid id)
    {
        _logger.LogInformation("Список паролей входов получен");
        return Ok(await _adminService.GetUserPasswords(id));
    }
    
}