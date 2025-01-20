using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Read.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Read.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Read.Application.DenormalizationEvents.Menus;
using RestaurantManagement.Read.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Read.Domain.Contracts.Repositories;
using RestaurantManagement.Read.Domain.Enums;
using RestaurantManagement.Read.Domain.Models;

namespace RestaurantManagement.Read.Application.DenormalizationEventHandlers;

public class RestaurantDenormalizationHandler :
    IEventHandler<RestaurantCreatedDenormalizationEvent>,
    IEventHandler<RestaurantUpdatedDenormalizationEvent>,
    IEventHandler<RestaurantActivatedDenormalizationEvent>,
    IEventHandler<RestaurantDeactivatedDenormalizationEvent>,
    IEventHandler<MenuCreatedDenormalizationEvent>,
    IEventHandler<MenuItemAddedDenormalizationEvent>,
    IEventHandler<MenuItemUpdatedDenormalizationEvent>,
    IEventHandler<MenuItemCategoryUpdatedDenormalizationEvent>,
    IEventHandler<MenuItemFoodTypesUpdatedDenormalizationEvent>,
    IEventHandler<MenuItemRemovedDenormalizationEvent>,
    IEventHandler<MenuItemStockUpdatedDenormalizationEvent>,
    IEventHandler<MenuCategoryUpdatedDenormalizationEvent>,
    IEventHandler<FoodTypeUpdatedDenormalizationEvent>
{
    private readonly IRestaurantRepository _repository;
    private readonly IMenuCategoryRepository _menuCategoryRepository;
    private readonly IFoodTypeRepository _foodTypeRepository;
    private readonly ILogger<RestaurantDenormalizationHandler> _logger;

    public RestaurantDenormalizationHandler(
        IRestaurantRepository repository,
        IMenuCategoryRepository menuCategoryRepository,
        IFoodTypeRepository foodTypeRepository,
        ILogger<RestaurantDenormalizationHandler> logger)
    {
        _repository = repository;
        _menuCategoryRepository = menuCategoryRepository;
        _foodTypeRepository = foodTypeRepository;
        _logger = logger;
    }

    public async Task HandleAsync(
        RestaurantCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.Id, cancellationToken))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        var restaurant = new Restaurant
        {
            Id = @event.Id,
            OwnerId = @event.OwnerId,
            Name = @event.Name,
            Status = @event.Status,
            Address = new Address(@event.Street, @event.City, @event.State, @event.ZipCode),
            Menus = [],
        };

        await _repository.AddAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        RestaurantUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.Id, @event.EventName, cancellationToken);

        restaurant.Name = @event.Name;
        restaurant.Address = new Address(@event.Street, @event.City, @event.State, @event.ZipCode);
        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        RestaurantActivatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.Id, @event.EventName, cancellationToken);
        restaurant.Status = RestaurantStatus.Active;
        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        RestaurantDeactivatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.Id, @event.EventName, cancellationToken);
        restaurant.Status = RestaurantStatus.Inactive;
        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(MenuCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        if (restaurant.Menus.Any(a => a.Id == @event.Id))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        restaurant.Menus.Add(new RestaurantMenu
        {
            Id = @event.Id,
            RestaurantId = restaurant.Id,
            MenuItems = []
        });

        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        MenuItemAddedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        var menu = GetMenu(restaurant, @event.MenuId, @event.EventName);
        var category = await GetMenuCategory(@event.CategoryId, @event.EventName, cancellationToken);
        var foodTypes = await GetFoodTypes(@event.FoodTypeIds, @event.EventName, cancellationToken);

        menu.MenuItems.Add(new RestaurantMenuItem
        {
            Id = @event.Id,
            CategoryId = @event.CategoryId,
            CategoryName = category.Name,
            Name = @event.Name,
            Price = @event.Price,
            Description = @event.Description,
            Stock = @event.Stock,
            FoodTypes = foodTypes.Select(s => new RestaurantFoodTypes() { Id = s.Id, Name = s.Name }).ToArray(),
        });

        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        MenuItemUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        var menuItem = GetMenuItem(restaurant, @event.Id, @event.EventName);
        menuItem.Name = @event.Name;
        menuItem.Price = @event.Price;
        menuItem.Description = @event.Description;

        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        MenuItemCategoryUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        var menuItem = GetMenuItem(restaurant, @event.Id, @event.EventName);
        var category = await GetMenuCategory(@event.CategoryId, @event.EventName, cancellationToken);

        menuItem.CategoryId = @event.CategoryId;
        menuItem.CategoryName = category.Name;

        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        MenuItemFoodTypesUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        var menuItem = GetMenuItem(restaurant, @event.Id, @event.EventName);
        var foodTypes = await GetFoodTypes(@event.FoodTypes, @event.EventName, cancellationToken);
        menuItem.FoodTypes = foodTypes
            .Select(s => new RestaurantFoodTypes() { Id = s.Id, Name = s.Name })
            .ToArray();

        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(MenuItemRemovedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);

        foreach (var menu in restaurant.Menus)
        {
            var menuItem = menu.MenuItems.SingleOrDefault(m => m.Id == @event.Id);
            if (menuItem != null)
            {
                menu.MenuItems.Remove(menuItem);
                await _repository.UpdateAsync(restaurant, cancellationToken);
                return;
            }
        }

        _logger.LogWarning("The event {eventName} seems to be denormalized", @event.EventName);
    }

    public async Task HandleAsync(
        MenuItemStockUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurant = await GetRestaurantQuery(@event.RestaurantId, @event.EventName, cancellationToken);
        var menuItem = GetMenuItem(restaurant, @event.Id, @event.EventName);

        menuItem.Stock = @event.Stock;
        await _repository.UpdateAsync(restaurant, cancellationToken);
    }

    public async Task HandleAsync(
        MenuCategoryUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurants = await _repository.GetByMenuCategoryIdAsync(@event.MenuCategoryId, cancellationToken);
        var menuItems = restaurants
            .SelectMany(s => s.Menus)
            .SelectMany(s => s.MenuItems)
            .Where(w => w.CategoryId == @event.MenuCategoryId);

        foreach (var menuItem in menuItems)
        {
            menuItem.CategoryName = @event.Name;
        }

        await _repository.UpdateManyAsync(restaurants, cancellationToken);
    }

    public async Task HandleAsync(FoodTypeUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var restaurants = await _repository.GetByFoodTypeIdAsync(@event.Id, cancellationToken);
        var foodTypes = restaurants
            .SelectMany(s => s.Menus)
            .SelectMany(s => s.MenuItems)
            .SelectMany(s => s.FoodTypes)
            .Where(w => w.Id == @event.Id);

        foreach (var foodType in foodTypes)
        {
            foodType.Name = @event.Name;
        }

        await _repository.UpdateManyAsync(restaurants, cancellationToken);
    }

    private async ValueTask<Restaurant> GetRestaurantQuery(
        Guid id, string eventName, CancellationToken cancellationToken = default)
    {
        var restaurant = await _repository.GetByIdAsync(id, cancellationToken);
        if (restaurant == null)
        {
            _logger.LogError(
                "Denormalization event {eventName} Failed. Restaurant with ID {id} not found", eventName, id);
            throw new EventIgnoredException();
        }

        return restaurant;
    }

    private RestaurantMenu GetMenu(Restaurant restaurant, Guid menuId, string eventName)
    {
        var menu = restaurant.Menus.SingleOrDefault(m => m.Id == menuId);
        if (menu == null)
        {
            _logger.LogError(
                "Denormalization event {eventName} Failed. Menu with ID {id} not found", eventName, menuId);
            throw new EventIgnoredException();
        }

        return menu;
    }

    private RestaurantMenuItem GetMenuItem(Restaurant restaurant, Guid menuItemId, string eventName)
    {
        var menuItem = restaurant.Menus.SelectMany(s => s.MenuItems).SingleOrDefault(m => m.Id == menuItemId);
        if (menuItem == null)
        {
            _logger.LogError(
                "Denormalization event {eventName} Failed. Menu item with ID {id} not found", eventName, menuItemId);
            throw new EventIgnoredException();
        }

        return menuItem;
    }

    private async ValueTask<MenuCategory> GetMenuCategory(
        Guid id, string eventName, CancellationToken cancellationToken = default)
    {
        var category = await _menuCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            _logger.LogError("The event {eventName} Failed, Category with ID {categoryId} not found",
                eventName,
                id);
            throw new EventIgnoredException();
        }

        return category;
    }

    private async ValueTask<ICollection<FoodType>> GetFoodTypes(
        ICollection<Guid> ids, string eventName, CancellationToken cancellationToken = default)
    {
        var foodTypes = await _foodTypeRepository.GetByIdsAsync(ids, cancellationToken);
        if (foodTypes.Count != ids.Count)
        {
            _logger.LogError("The event {eventName} Failed, Food types with IDs {ids} not found",
                eventName,
                string.Join(", ", ids));
            throw new EventIgnoredException();
        }

        return foodTypes;
    }
}
