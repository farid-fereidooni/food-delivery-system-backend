using Humanizer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RestaurantManagement.Domain.Contracts;

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

    private void Configure()
    {
        var enumPack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };
        ConventionRegistry.Register("EnumStringConvention", enumPack, t => true);

        BsonSerializer.RegisterSerializer(GuidSerializer.StandardInstance);
        BsonClassMap.RegisterClassMap<Storable>(c =>
        {
            c.AutoMap();
            c.MapIdMember(m => m.Id);
            c.SetIsRootClass(true);
        });
    }
}
