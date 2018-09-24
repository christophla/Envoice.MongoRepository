using System;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Configures the mongo db driver conventions.
    /// </summary>
    internal static class MongoConfig
    {
        private static bool _initialized = false;
        private static object _initializationLock = new object();
        private static object _initializationTarget;

        /// <summary>
        /// Ensures that the conventions have been configured.
        /// </summary>
        public static void EnsureConfigured()
        {
            LazyInitializer.EnsureInitialized(ref _initializationTarget, ref _initialized, ref _initializationLock, () =>
            {
                Configure();
                return null;
            });
        }

        #region [ Private Methods ]

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
            //TODO: Make these configurable at runtime
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

        #endregion
    }
}
