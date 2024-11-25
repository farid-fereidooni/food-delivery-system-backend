using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Api.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public Result<Guid> CurrentUserId()
    {
        var subClaim = GetSubClaim();
        if (subClaim == null)
            return new Error(CommonResource.Authorization_NotAuthenticated).WithReason(ErrorReason.NotAuthenticated);

        return Guid.Parse(subClaim);
    }

    private string? GetSubClaim()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
    }
}
