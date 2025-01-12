using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Domain.Contracts;

public interface IAuthService
{
    bool IsAuthenticated();
    Result<Guid> GetCurrentUserId();
}
