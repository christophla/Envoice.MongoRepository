using System;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Envoice.MongoRepository.Impl
{
  internal static class MongoConfig
  {
    private static bool _initialized = false;
    private static object _initializationLock = new object();
    private static object _initializationTarget;

    public static void EnsureConfigured()
    {
      LazyInitializer.EnsureInitialized(ref _initializationTarget, ref _initialized, ref _initializationLock, () =>
      {
        Configure();
        return null;
      });
    }

    private static void Configure()
    {
      RegisterConventions();

      BsonClassMap.RegisterClassMap<Entity>(cm =>
      {
        cm.AutoMap();
        cm.MapIdMember(c => c.Id)
            .SetSerializer(new StringSerializer(BsonType.ObjectId))
            .SetIdGenerator(StringObjectIdGenerator.Instance);
      });

    }

    private static void RegisterConventions()
    {
      var pack = new ConventionPack
            {
                new IgnoreIfNullConvention(false),
                new CamelCaseElementNameConvention(),
            };

      ConventionRegistry.Register("Envoice.MongoRepository", pack, IsConventionApplicable);
    }

    private static bool IsConventionApplicable(Type type)
    {
      return type == typeof(Entity);
    }
  }
}