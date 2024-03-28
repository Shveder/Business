using Business_monitoring.DTO;
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
    ///     Get /Admin/GetAllUsers
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
    ///     Get /Admin/GetAllCompanies
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
    ///     Get /Admin/GetAllExperts
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
    ///     Get /Admin/GetUserLoginHistory
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
    ///     Get /Admin/GetUserRecentPasswords
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
    
    /// <summary>
    ///     Delete user from list
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/DeleteUser
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Deletes user.</response>
    [HttpDelete("DeleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        _logger.LogInformation("Пользователь удален");
        await _adminService.DeleteUser(id);
        return Ok("Пользователь удален");
    }
    
    /// <summary>
    ///     Delete company from list
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/DeleteCompany
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Deletes company.</response>
    [HttpDelete("DeleteCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        _logger.LogInformation("Компания удалена");
        await _adminService.DeleteCompany(id);
        return Ok("Компания удалена");
    }
    
    /// <summary>
    ///     Delete expert from list
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/DeleteExpert
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Deletes expert.</response>
    [HttpDelete("DeleteExpert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteExpert(Guid id)
    {
        _logger.LogInformation("Эксперт удален");
        await _adminService.DeleteExpert(id);
        return Ok("Эксперт удален");
    }
    
    /// <summary>
    ///     Change user role
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/ChangeRole
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Changes user role.</response>
    /// <response code="422">No such user.</response>
    [HttpPut("ChangeRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleRequest request) // 1 - user, 0 - admin
    {
        _logger.LogInformation("Роль изменена");
        await _adminService.ChangeRole(request);
        return Ok("Роль изменена");
    }
    
    /// <summary>
    ///     Sets user block status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetUserBlockStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets user block status.</response>
    /// <response code="422">No such user.</response>
    [HttpPut("SetUserBlockStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetUserBlockStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус блокировки изменен");
        await _adminService.SetUserBlockStatus(request);
        return Ok("Статус блокировки изменен");
    }
    
    /// <summary>
    ///     Sets company block status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetCompanyBlockStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets company block status.</response>
    /// <response code="422">No such company.</response>
    [HttpPut("SetCompanyBlockStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetCompanyBlockStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус блокировки изменен");
        await _adminService.SetCompanyBlockStatus(request);
        return Ok("Статус блокировки изменен");
    }
    
    /// <summary>
    ///     Sets expert block status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetExpertBlockStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets expert block status.</response>
    /// <response code="422">No such expert.</response>
    [HttpPut("SetExpertBlockStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetExpertBlockStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус блокировки изменен");
        await _adminService.SetExpertBlockStatus(request);
        return Ok("Статус блокировки изменен");
    }
    
    /// <summary>
    ///     Sets user delete status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetUserDeleteStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets user delete status.</response>
    /// <response code="422">No such user.</response>
    [HttpPut("SetUserDeleteStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetUserDeleteStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус удаления изменен");
        await _adminService.SetUserDeleteStatus(request);
        return Ok("Статус удаления изменен");
    }
    
    /// <summary>
    ///     Sets company delete status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetCompanyDeleteStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets company delete status.</response>
    /// <response code="422">No such company.</response>
    [HttpPut("SetCompanyDeleteStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetCompanyDeleteStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус удаления изменен");
        await _adminService.SetCompanyDeleteStatus(request);
        return Ok("Статус удаления изменен");
    }
    
    /// <summary>
    ///     Sets expert delete status
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     Delete /Admin/SetExpertDeleteStatus
    /// </remarks>
    /// <returns>
    ///     200 OK 
    /// </returns>
    /// <response code="200">Sets expert delete status.</response>
    /// <response code="422">No such expert.</response>
    [HttpPut("SetExpertDeleteStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetExpertDeleteStatus([FromBody] ChangeStatusRequest request)
    {
        _logger.LogInformation("Статус удаления изменен");
        await _adminService.SetExpertDeleteStatus(request);
        return Ok("Статус удаления изменен");
    }
    
}