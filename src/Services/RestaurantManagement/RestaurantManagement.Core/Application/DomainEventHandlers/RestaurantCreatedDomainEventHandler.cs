using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Contracts.Command;
using RestaurantManagement.Core.Domain.DomainEvents;
using RestaurantManagement.Core.Domain.Models.MenuAggregate;

namespace RestaurantManagement.Core.Application.DomainEventHandlers;

public class RestaurantCreatedDomainEventHandler : INotificationHandler<RestaurantCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuCommandRepository _menuRepository;

    public RestaurantCreatedDomainEventHandler(IUnitOfWork unitOfWork,
        IMenuCommandRepository menuRepository)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
    }

    public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
    {
        var defaultMenu = new Menu(notification.RestaurantId);

        await _menuRepository.AddAsync(defaultMenu, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
