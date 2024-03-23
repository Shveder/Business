using Business_monitoring.DTO;
using Business_monitoring.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Business_monitoring.Controllers;


[ApiController]
[Route("[controller]")]

public class AuthorizationController : ControllerBase
{
    
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IUserService _userService;

    public AuthorizationController(ILogger<AuthorizationController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        if (request.Password != request.RepeatPassword)
        {
            return UnprocessableEntity("Пароли не совпадают");
        }
        await _userService.CreateUserAsync(request);
        _logger.LogInformation($"Пользователь с логином {request.Login} создан");
        return Ok($"Пользователь с логином {request.Login} создан");
    }

}