using RestaurantManagement.Core.Domain.Dtos;

namespace RestaurantManagement.Core.Domain.Contracts;

public interface IAuthService
{
    Task<bool> IsAuthenticated();
    Task<Result<Guid>> CurrentUserId();
}
