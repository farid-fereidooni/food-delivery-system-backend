using MediatR;

namespace RestaurantManagement.Application.Command;

public interface IQuery<out TResponse> : IRequest<TResponse>;
