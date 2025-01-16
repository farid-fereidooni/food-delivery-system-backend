using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Domain.Contracts;

public interface IAuthService
{
    bool IsAuthenticated();
    Result<Guid> GetCurrentUserId();
}
