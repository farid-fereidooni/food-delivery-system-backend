using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Dtos;
using RestaurantManagement.Domain.Exceptions;
using RestaurantManagement.Domain.Resources;
using RestaurantManagement.Domain.ValueObjects;

namespace RestaurantManagement.Domain.Models.RestaurantAggregate;

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

    protected internal EntityCollection<Restaurant> Restaurants { get; } = new();

    public Result CanAddRestaurant(string name, Address address)
    {
        var error = new Error();

        if (Restaurants.Count >= MaxRestaurantCount)
            error.AddMessage(CommonResource.Validation_MaximumNumberOfRestaurant);

        if (Restaurants.Any(x => x.Name == name))
            error.AddMessage(CommonResource.Validation_RestaurantNameShouldBeUnique);

        return error.IsEmpty ? Result.Success() : error;
    }

    public Guid AddRestaurant(string name, Address address)
    {
        var validation = CanAddRestaurant(name, address);
        InvalidDomainStateException.ThrowIfError(validation);

        var restaurant = new Restaurant(name, address, Id);
        Restaurants.Add(restaurant);

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
        return Restaurants.TryGetById(restaurantId, out var restaurant)
            ? restaurant
            : throw InvalidDomainOperationException.Create("Restaurant does not exist.");
    }

    public bool HasRestaurants() => Restaurants.Count != 0;
}
