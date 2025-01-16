using MediatR;

namespace RestaurantManagement.Write.Application.Command;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;
