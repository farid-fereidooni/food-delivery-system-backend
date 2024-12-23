using Humanizer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Infrastructure.Database.Query;

public class QueryDbContext
{
    private readonly IMongoClient _mongoClient;
    private const string DatabaseName = "RestaurantManagement";
    private Dictionary<Type, string> CollectionNames { get; } = new();

    public QueryDbContext(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
        Configure();
    }

    public IMongoCollection<T> GetCollection<T>() where T : StorableRoot
    {
        return _mongoClient.GetDatabase(DatabaseName).GetCollection<T>(GetCollectionName<T>());
    }

    private string GetCollectionName<T>()
    {
        if (CollectionNames.TryGetValue(typeof(T), out var collectionName))
        {
            return collectionName;
        }
        var name = typeof(T).Name.Pluralize().Camelize();
        CollectionNames[typeof(T)] = name;
        return name;
    }

    private static void Configure()
    {
        BsonSerializer.RegisterSerializer(GuidSerializer.StandardInstance);
        BsonClassMap.RegisterClassMap<Storable>(c =>
        {
            c.AutoMap();
            c.MapIdMember(m => m.Id);
            c.SetIsRootClass(true);
        });

        BsonClassMap.RegisterClassMap<MenuCategoryQuery>();
    }
}
