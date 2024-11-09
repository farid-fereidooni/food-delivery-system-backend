namespace RestaurantManagement.Core.Domain.Contracts;

public interface IAuthService
{
    Task<bool> IsAuthenticated();
    Task<Guid> CurrentUserId();
}
