using System.Security.Claims;

namespace riga.services.riga.services.authentication.Services.Guard;

public class AuthGuard
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthGuard(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        return null;
    }
}