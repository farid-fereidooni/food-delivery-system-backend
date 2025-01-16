using MediatR;

namespace RestaurantManagement.Read.Application.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>;
