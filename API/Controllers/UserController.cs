using System.Security.Claims;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Infrastructure.Extensions.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LayeredAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly string _jwtSecretKey;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        IConfigurationSection appSettings = configuration.GetSection("AppSettings");
        _jwtSecretKey = appSettings.GetSection("JWTKey").Value;
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


    [AuthorizeWithClaim(ClaimTypes.Role, "SuperAdmin")]
    [HttpPost("registerByAdmin")]
    public async Task<IActionResult> RegisterBySuperAdmin([FromBody] RegisterUserByAdminRequest registerUserRequest)
    {
        try
        {
            var response = await _userService.RegisterByAdminAsync(registerUserRequest);

            return Ok(response);
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

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var response = await _userService.GetUsers();

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }
}