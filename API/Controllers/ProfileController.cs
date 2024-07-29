using LayeredAPI.Domain.Interfaces.Services;
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
    [HttpGet("/{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] int id)
    {
        try
        {
            var response = await _profileService.GetProfile(id);

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong");
        }
    }
}