using System.Security.Claims;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LayeredAPI.Controllers;

[ApiController]
[Route("profile")]

public class ProfileController: ControllerBase
{
    
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID is not available in claims.");
            }
            
            var response = await _profileService.GetProfile(userId);

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }


    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest editProfileRequest)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID is not available in claims.");
            }

            var response = await _profileService.EditProfile(userId, editProfileRequest);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }
    
}