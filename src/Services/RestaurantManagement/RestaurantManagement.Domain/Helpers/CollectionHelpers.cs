namespace RestaurantManagement.Domain.Helpers;

public static class CollectionHelpers
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Remove(item);
        }
    }

    public static bool ContainedIn<T>(this T item, IEnumerable<T> collection)
    {
        return collection.Contains(item);
    }
}
