using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;


[ApiController]
[Route("[controller]")]
[Produces("application/json")]

public class AuthorizationController : ControllerBase
{
    
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IUserService _userService;

    public AuthorizationController(ILogger<AuthorizationController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    /// <summary>
    ///     Register one user
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     GET /Authorization/user
    /// </remarks>
    /// <returns>
    ///     200 OK with the user data.
    /// </returns>
    /// <response code="200">Returns the list of user data.</response>
    /// /// <response code="422">Invalid user data</response>
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        await _userService.CreateUserAsync(request);
        _logger.LogInformation($"Пользователь с логином {request.Login} создан");
        return Ok($"Пользователь с логином {request.Login} создан");
    }
    
    /// <summary>
    ///     Login one user
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     GET /Authorization/user
    /// </remarks>
    /// <returns>
    ///     200 OK with the user data.
    /// </returns>
    /// <response code="200">Returns the list of user data.</response>
    /// /// <response code="422">Invalid user data</response>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.Login(request);
        _logger.LogInformation($"Пользователь с логином {request.Login} вошел в систему");
        return Ok(user);
    }

}