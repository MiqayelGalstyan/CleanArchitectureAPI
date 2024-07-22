using LayeredAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LayeredAPI.Controllers;

[ApiController]
[Route("roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize]
    [HttpGet("/role/{id}")]
    public async Task<IActionResult> GetRoleById([FromRoute] int id)
    {
        try
        {
            var role = await _roleService.GetRoleById(id);
            return Ok(role);
        }
        catch (InvalidOperationException)
        {
            return Unauthorized("Invalid username or password");
        }
    }
}