using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IAuthService
{
    bool IsAuthenticated();
    Result<Guid> CurrentUserId();
}
