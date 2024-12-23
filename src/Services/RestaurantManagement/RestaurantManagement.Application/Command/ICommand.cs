using MediatR;

namespace RestaurantManagement.Application.Command;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;
