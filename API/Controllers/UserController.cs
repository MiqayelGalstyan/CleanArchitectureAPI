using System.Security.Claims;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Infrastructure.Extensions.Attributes;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            string token = refreshTokenRequest.RefreshToken;

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Refresh token is required.");
            }

            if (_jwtSecretKey != null)
            {
                var response = await _userService.RefreshToken(token, _jwtSecretKey);

                return Ok(response);
            }

            return BadRequest("Problem with JWT secret key");
        }
        catch (InvalidOperationException)
        {
            return BadRequest("Something went wrong");
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
            throw new Exception("Something went wrong");
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
            throw new Exception("Something went wrong");
        }
    }


    [AuthorizeWithClaim(ClaimTypes.Role, "SuperAdmin")]
    [HttpDelete("/user/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {

        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (currentUserId == id)
        {
            return BadRequest("You cannot delete yourself.");
        }
        
        try
        {
            var isDeleted = await _userService.DeleteUser(id);
            return Ok(isDeleted);
        }
        catch (InvalidOperationException)
        {
            throw new Exception("Something went wrong");
        }
    }


    [Authorize]
    [HttpGet("/user/{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] int id)
    {
        try
        {
            var response = await _userService.GetUser(id);

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }


    [Authorize]
    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest getUsersRequest)
    {
        try
        {
            var response = await _userService.GetUsers(getUsersRequest);

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }
}