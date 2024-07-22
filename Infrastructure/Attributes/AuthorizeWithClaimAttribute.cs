using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace LayeredAPI.Infrastructure.Extensions.Attributes;

public class AuthorizeWithClaimAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _claimType;
    private readonly string _claimValue;

    public AuthorizeWithClaimAttribute(string claimType, string claimValue)
    {
        _claimType = claimType;
        _claimValue = claimValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasClaim = user.Claims.Any(c => c.Type == _claimType && c.Value == _claimValue);
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
        }
    }
}
