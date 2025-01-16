using RestaurantManagement.Read.Domain.Dtos;

namespace RestaurantManagement.Read.Domain.Contracts;

public interface IAuthService
{
    bool IsAuthenticated();
    Result<Guid> GetCurrentUserId();
}
