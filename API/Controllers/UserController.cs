using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace LayeredAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly string? _jwtSecretKey;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _jwtSecretKey = configuration.GetSection("AppSettings:JWTKey").Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            if (_jwtSecretKey != null)
            {
                var response = await _userService.Login(loginRequest, _jwtSecretKey);
                
                 return Ok(response);
            }
            return BadRequest("Problem with JWT secret key");
        }
        catch (InvalidOperationException)
        {
            return Unauthorized("Invalid username or password");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
    {
        try
        {
                var response = await _userService.RegisterAsync(registerUserRequest);
                
                return Ok(response);
        }
        catch (InvalidOperationException)
        {
            return Unauthorized("Invalid username or password");
        }
    }
}