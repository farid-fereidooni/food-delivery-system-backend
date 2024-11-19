using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.Exceptions;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Domain.Models.RestaurantAggregate;

public class RestaurantOwner : AggregateRoot
{
    private const int MaxRestaurantCount = 1;
    protected RestaurantOwner()
    {
    }

    public RestaurantOwner(Guid ownerId)
    {
        Id = ownerId;
    }

    private IDictionary<Guid, Restaurant> _restaurantDictionary = new Dictionary<Guid, Restaurant>();
    private ICollection<Restaurant> Restaurants {
        get => _restaurantDictionary.Values;
        set { _restaurantDictionary = value.ToDictionary(x => x.Id, x => x); }
    }

    public Result CanAddRestaurant(string name, Address address)
    {
        var error = new Error();

        if (_restaurantDictionary.Count >= MaxRestaurantCount)
            error.AddMessage(CommonResource.Validation_MaximumNumberOfRestaurant, nameof(CommonResource.Validation_MaximumNumberOfRestaurant));

        if (_restaurantDictionary.Values.Any(x => x.Name == name))
            error.AddMessage(CommonResource.Validation_RestaurantNameShouldBeUnique, nameof(CommonResource.Validation_RestaurantNameShouldBeUnique));

        return error.IsEmpty ? Result.Success() : error;
    }

    public Guid AddRestaurant(string name, Address address)
    {
        var validation = CanAddRestaurant(name, address);
        InvalidDomainStateException.ThrowIfError(validation);

        var restaurant = new Restaurant(name, address, Id);
        _restaurantDictionary.Add(restaurant.Id, restaurant);

        return restaurant.Id;
    }

    public void UpdateRestaurantInfo(Guid restaurantId, string newName, Address newAddress)
    {
        var restaurant = GetRestaurant(restaurantId);
        restaurant.UpdateInfo(newName, newAddress);
    }

    public void ActivateRestaurant(Guid restaurantId)
    {
        var restaurant = GetRestaurant(restaurantId);
        restaurant.Activate();
    }

    public void DeactivateRestaurant(Guid restaurantId)
    {
        var restaurant = GetRestaurant(restaurantId);
        restaurant.Deactivate();
    }

    private Restaurant GetRestaurant(Guid restaurantId)
    {
        return _restaurantDictionary.TryGetValue(restaurantId, out var restaurant)
            ? restaurant
            : throw InvalidDomainOperationException.Create("Restaurant does not exist.");
    }

    public bool HasRestaurants() => Restaurants.Count != 0;
}