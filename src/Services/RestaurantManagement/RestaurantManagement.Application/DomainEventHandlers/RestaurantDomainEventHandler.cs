using MediatR;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Contracts.Command;
using RestaurantManagement.Domain.DomainEvents;
using RestaurantManagement.Domain.Models.Command.MenuAggregate;

namespace RestaurantManagement.Application.DomainEventHandlers;

public class RestaurantDomainEventHandler : INotificationHandler<RestaurantCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;

    public RestaurantDomainEventHandler(IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
    }

    public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
    {
        var defaultMenu = new Menu(notification.RestaurantId);

        await _menuRepository.AddAsync(defaultMenu, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
