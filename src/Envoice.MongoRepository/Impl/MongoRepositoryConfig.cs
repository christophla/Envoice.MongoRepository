using System;
using System.Security.Authentication;
using Envoice.Conditions;
using Envoice.MongoRepository.Helpers;
using MongoDB.Driver;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Configuration for mongo repository.
    /// </summary>
    public class MongoRepositoryConfig
    {
        /// <summary>
        /// Indicates if virtual collections are enabled.
        /// </summary>
        public bool VirtualCollectionEnabled { get; }

        /// <summary>
        /// Indicates if the default virtual collection should be used for all entities.
        /// </summary>
        public bool VirtualCollectionForceGlobal { get; }

        /// <summary>
        /// The default virtual collection for unmapped entities.code
        /// </summary>
        public string VirtualCollectionDefault { get; }

        /// <summary>
        /// Indicates if the default virtual collection has been set.
        /// </summary>
        public bool HasVirtualCollectionDefault => !string.IsNullOrWhiteSpace(this.VirtualCollectionDefault);

        /// <summary>
        /// The configured connection string
        /// </summary>
        public string ConnectionString { get; }

        public MongoRepositoryConfig(string connectionString)
        {
            Condition.Requires(connectionString, "connectionString").IsNotNullOrWhiteSpace();
            this.ConnectionString = connectionString;

            // Init properties
            var queryString = new Uri(connectionString).Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            this.VirtualCollectionDefault = queryDictionary["virtualCollection"];
            this.VirtualCollectionEnabled = ConversionUtil.Bool(queryDictionary["virtual"]);
            this.VirtualCollectionForceGlobal = ConversionUtil.Bool(queryDictionary["virtualCollectionGlobal"]);

        }

        /// <summary>
        /// Returns the mongo collection for a given type.
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T, TKey>() where T : IEntity<TKey>
        {
            var collectionName = GetCollectionName<T, TKey>();
            var collection = GetDatabaseFromUrl(new MongoUrl(this.ConnectionString)).GetCollection<T>(collectionName);

            if (this.VirtualCollectionEnabled)
                collection = new VirtualCollection<T, TKey>(collection, this);

            return collection;
        }

        #region [ Private Methods ]

        /// <summary>
        /// Creates and returns a MongoDatabase from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the database from.</param>
        /// <returns>Returns a MongoDatabase from the specified url.</returns>
        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var settings = MongoClientSettings.FromUrl(url);
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var client = new MongoClient(settings);
            return client.GetDatabase(url.DatabaseName); // WriteConcern defaulted to Acknowledged
        }


        /// <summary>
        /// Determines the collectionname for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collectionname for.</typeparam>
        /// <returns>Returns the collectionname for T.</returns>
        private string GetCollectionName<T, TKey>()
            where T : IEntity<TKey>
        {
            // global virtual collection override
            if (this.VirtualCollectionEnabled && this.VirtualCollectionForceGlobal)
            {
                if (string.IsNullOrWhiteSpace(this.VirtualCollectionDefault))
                    throw new Exception("You must define a default virtual collection name in the connection string.");

                return this.VirtualCollectionDefault;
            }

            // collection name from type or interface
            string collectionName;

            if (typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            Condition.Ensures(collectionName).IsNotNullOrWhiteSpace("Collection name cannot be empty for this entity");

            return collectionName;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collectionname from.</typeparam>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private string GetCollectioNameFromInterface<T>()
        {
            string collectionName;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var collectionNameAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            var virtualCollectionNameAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(VirtualCollectionName));

            // Cannot define both
            if (collectionNameAttribute != null && virtualCollectionNameAttribute != null)
            {
                throw new Exception($"CollectionName and VirtualCollectionName cannot both be defined on {typeof(T).Name}");
            }

            // Parse collection name
            if (collectionNameAttribute != null)
            {
                collectionName = ((CollectionName)collectionNameAttribute).Name;
            }
            else if (this.VirtualCollectionEnabled && virtualCollectionNameAttribute != null)
            {
                collectionName = ((VirtualCollectionName)virtualCollectionNameAttribute).Name;
            }
            else if (this.VirtualCollectionEnabled && this.HasVirtualCollectionDefault)
            {
                collectionName = this.VirtualCollectionDefault;
            }
            else
            {
                collectionName = typeof(T).Name;
            }

            return collectionName;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <param name="entityType">The type of the entity to get the collectionname from.</param>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private string GetCollectionNameFromType(Type entityType)
        {
            string collectionName;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var collectionNameAttribute = Attribute.GetCustomAttribute(entityType, typeof(CollectionName));
            var virtualCollectionNameAttribute = Attribute.GetCustomAttribute(entityType, typeof(VirtualCollectionName));

            // Cannot define both
            if (collectionNameAttribute != null && virtualCollectionNameAttribute != null)
            {
                throw new Exception($"CollectionName and VirtualCollectionName cannot both be defined on {entityType.Name}");
            }

            // Parse collection name
            if (collectionNameAttribute != null)
            {
                collectionName = ((CollectionName)collectionNameAttribute).Name;
            }
            else if (this.VirtualCollectionEnabled && virtualCollectionNameAttribute != null)
            {
                collectionName = ((VirtualCollectionName)virtualCollectionNameAttribute).Name;
            }
            else if (this.VirtualCollectionEnabled && this.HasVirtualCollectionDefault)
            {
                collectionName = this.VirtualCollectionDefault;
            }
            else
            {
                if (typeof(Entity).IsAssignableFrom(entityType))
                {
                    // No attribute found, get the basetype
                    while (!entityType.BaseType.Equals(typeof(Entity)))
                    {
                        entityType = entityType.BaseType;
                    }
                }
                collectionName = entityType.Name;
            }

            return collectionName;
        }
        #endregion

    }
}
