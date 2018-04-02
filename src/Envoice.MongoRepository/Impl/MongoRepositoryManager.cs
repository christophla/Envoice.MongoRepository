using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using System.Threading.Tasks;
using Envoice.MongoRepository.Helpers;
using Envoice.Conditions;
using System.Threading;
using MongoDB.Bson.Serialization.Conventions;
using System.Linq.Expressions;

namespace Envoice.MongoRepository
{

    /// <summary>
    /// Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    /// as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    /// objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepositoryManager<T, TKey> : IRepositoryManager<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        private IMongoCollection<T> collection;

        /// <summary>
        /// Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        public virtual bool Exists
        {
            get { return this.collection.Database.ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", this.Name) }).ToList().Any(); }
        }

        /// <summary>
        /// Gets the name of the collection as Mongo uses.
        /// </summary>
        /// <value>The name of the collection as Mongo uses.</value>
        public virtual string Name
        {
            get { return this.collection.CollectionNamespace.CollectionName; }
        }

        /// <summary>
        /// Initializes the mongo bson mappings
        /// </summary>
        static MongoRepositoryManager()
        {
            MongoConfig.EnsureConfigured();
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// Uses the Default App/Web.Config connection strings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connection string.</remarks>
        public MongoRepositoryManager() : this(Configuration.Database.ConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connection string to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString) : this(new MongoRepositoryConfig(connectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public MongoRepositoryManager(MongoRepositoryConfig config)
        {
            Condition.Requires(config, "config").IsNotNull();

            this.collection = config.GetCollection<T, TKey>();
        }

        /// <summary>
        /// Drops the collection.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Drop(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.Database.DropCollection(this.Name, cancellationToken);
        }

        /// <summary>
        /// Drops the collection.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DropAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.Database.DropCollectionAsync(this.Name, cancellationToken);
        }

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void DropAllIndexes(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.Indexes.DropAll(cancellationToken);
        }

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DropAllIndexesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.Indexes.DropAllAsync(cancellationToken);
        }

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void DropIndex(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNullOrWhiteSpace();

            this.DropIndexes(new string[] { keyname }, cancellationToken);
        }

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DropIndexAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNullOrWhiteSpace();

            await this.DropIndexesAsync(new string[] { keyname }, cancellationToken);
        }

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void DropIndexes(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            foreach (var name in keynames)
            {
                this.collection.Indexes.DropOne(name, cancellationToken);
            }
        }

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DropIndexesAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            foreach (var name in keynames)
            {
                await this.collection.Indexes.DropOneAsync(name, cancellationToken);
            }
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public string EnsureIndex(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EnsureIndex(new[] { keyname }, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public string EnsureIndex(Expression<Func<T, object>> keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EnsureIndex(new[] { keyname }, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public async Task<string> EnsureIndexAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return await EnsureIndexAsync(new[] { keyname }, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public async Task<string> EnsureIndexAsync(Expression<Func<T, object>> keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return await EnsureIndexAsync(new[] { keyname }, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public string EnsureIndex(string keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return EnsureIndex(new[] { keyname }, descending, unique, sparse, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public string EnsureIndex(Expression<Func<T, object>> keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return EnsureIndex(new[] { keyname }, descending, unique, sparse, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public async Task<string> EnsureIndexAsync(string keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return await EnsureIndexAsync(new[] { keyname }, descending, unique, sparse, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public async Task<string> EnsureIndexAsync(Expression<Func<T, object>> keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNull();

            return await EnsureIndexAsync(new[] { keyname }, descending, unique, sparse, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public string EnsureIndex(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            return EnsureIndex(keynames, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public string EnsureIndex(IEnumerable<Expression<Func<T, object>>> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            return EnsureIndex(keynames, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public async Task<string> EnsureIndexAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            return await EnsureIndexAsync(keynames, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public async Task<string> EnsureIndexAsync(IEnumerable<Expression<Func<T, object>>> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            return await EnsureIndexAsync(keynames, false, false, false, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual string EnsureIndex(IEnumerable<string> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var options = new CreateIndexOptions { Sparse = sparse, Unique = unique };
            var builder = Builders<T>.IndexKeys;
            IndexKeysDefinition<T> indexKeysDefinition = null;

            foreach (var k in keynames)
            {
                if (descending)
                {
                    if (null == indexKeysDefinition)
                        indexKeysDefinition = builder.Descending(k);
                    else
                        indexKeysDefinition = indexKeysDefinition.Descending(k);
                }
                else
                  if (null == indexKeysDefinition)
                    indexKeysDefinition = builder.Ascending(k);
                else
                    indexKeysDefinition = indexKeysDefinition.Ascending(k);
            }

            return this.collection.Indexes.CreateOne(indexKeysDefinition, options, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual string EnsureIndex(IEnumerable<Expression<Func<T, object>>> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var options = new CreateIndexOptions { Sparse = sparse, Unique = unique };
            var builder = Builders<T>.IndexKeys;
            IndexKeysDefinition<T> indexKeysDefinition = null;

            foreach (var k in keynames)
            {
                if (descending)
                {
                    if (null == indexKeysDefinition)
                        indexKeysDefinition = builder.Descending(k);
                    else
                        indexKeysDefinition = indexKeysDefinition.Descending(k);
                }
                else
                  if (null == indexKeysDefinition)
                    indexKeysDefinition = builder.Ascending(k);
                else
                    indexKeysDefinition = indexKeysDefinition.Ascending(k);
            }

            return this.collection.Indexes.CreateOne(indexKeysDefinition, options, cancellationToken);
        }


        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public async virtual Task<string> EnsureIndexAsync(IEnumerable<string> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var options = new CreateIndexOptions { Unique = unique, Sparse = sparse };
            var builder = Builders<T>.IndexKeys;
            IndexKeysDefinition<T> indexKeysDefinition = null;

            foreach (var k in keynames)
            {
                if (descending)
                {
                    if (null == indexKeysDefinition)
                        indexKeysDefinition = builder.Descending(k);
                    else
                        indexKeysDefinition = indexKeysDefinition.Descending(k);
                }
                else
                  if (null == indexKeysDefinition)
                    indexKeysDefinition = builder.Ascending(k);
                else
                    indexKeysDefinition = indexKeysDefinition.Ascending(k);
            }

            return await this.collection.Indexes.CreateOneAsync(indexKeysDefinition, options, cancellationToken);
        }

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public async virtual Task<string> EnsureIndexAsync(IEnumerable<Expression<Func<T, object>>> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var options = new CreateIndexOptions { Unique = unique, Sparse = sparse };
            var builder = Builders<T>.IndexKeys;
            IndexKeysDefinition<T> indexKeysDefinition = null;

            foreach (var k in keynames)
            {
                if (descending)
                {
                    if (null == indexKeysDefinition)
                        indexKeysDefinition = builder.Descending(k);
                    else
                        indexKeysDefinition = indexKeysDefinition.Descending(k);
                }
                else
                  if (null == indexKeysDefinition)
                    indexKeysDefinition = builder.Ascending(k);
                else
                    indexKeysDefinition = indexKeysDefinition.Ascending(k);
            }

            return await this.collection.Indexes.CreateOneAsync(indexKeysDefinition, options, cancellationToken);
        }

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns the indexes for this repository.</returns>
        public virtual List<BsonDocument> GetIndexes(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.collection.Indexes.List(cancellationToken).ToList();
        }

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns the indexes for this repository.</returns>
        public async virtual Task<List<BsonDocument>> GetIndexesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.collection.Indexes.List(cancellationToken).ToListAsync();
        }

        /// <summary>
        /// Gets stats for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a CollectionStatsResult.</returns>∏
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual CollectionStatsResult GetStats(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CollectionStatsResult(
                this.collection.Database.RunCommand(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "collstats", this.Name } }), null, cancellationToken)
            );
        }

        /// <summary>
        /// Gets stats for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a CollectionStatsResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public async virtual Task<CollectionStatsResult> GetStatsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CollectionStatsResult(
                await this.collection.Database.RunCommandAsync(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "collstats", this.Name } }), null, cancellationToken)
            );
        }

        /// <summary>
        /// Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual long GetTotalDataSize(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetStats(cancellationToken).DataSize;
        }


        /// <summary>
        /// Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public async virtual Task<long> GetTotalDataSizeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var stats = await this.GetStatsAsync(cancellationToken);
            return stats.DataSize;
        }

        /// <summary>
        /// Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual long GetTotalStorageSize(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetStats(cancellationToken).StorageSize;
        }

        /// <summary>
        /// Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public async virtual Task<long> GetTotalStorageSizeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var stats = await this.GetStatsAsync(cancellationToken);
            return stats.StorageSize;
        }


        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual bool IndexesExists(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var ix = this.collection.Indexes.List(cancellationToken).ToList();
            return keynames.All(k => ix.Contains(BsonValue.Create(k)));
        }

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public async virtual Task<bool> IndexesExistsAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keynames, "keynames").IsNotNull();

            var ix = await this.collection.Indexes.List(cancellationToken).ToListAsync();
            return keynames.All(k => ix.Contains(BsonValue.Create(k)));
        }

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual bool IndexExists(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNullOrWhiteSpace();

            return this.IndexesExists(new string[] { keyname }, cancellationToken);
        }

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public async virtual Task<bool> IndexExistsAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken))
        {
            Condition.Requires(keyname, "keyname").IsNotNullOrWhiteSpace();

            return await this.IndexesExistsAsync(new string[] { keyname }, cancellationToken);
        }

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        public virtual bool IsCapped(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetStats(cancellationToken).IsCapped;
        }

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        public async virtual Task<bool> IsCappedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var stats = await this.GetStatsAsync(cancellationToken);
            return stats.IsCapped;
        }

        /// <summary>
        /// Runs the ReIndex command on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void ReIndex(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.Database.RunCommand(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "reIndex", this.Name } }), null, cancellationToken);
        }

        /// <summary>
        /// Runs the ReIndex command on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task ReIndexAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.Database.RunCommandAsync(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "reIndex", this.Name } }), null, cancellationToken);
        }

        /// <summary>
        /// Validates the integrity of the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual ValidateCollectionResult Validate(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new ValidateCollectionResult(
                this.collection.Database.RunCommand(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "validate", this.Name } }), null, cancellationToken)
            );
        }

        /// <summary>
        /// Validates the integrity of the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public async virtual Task<ValidateCollectionResult> ValidateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new ValidateCollectionResult(
                await this.collection.Database.RunCommandAsync(new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "validate", this.Name } }), null, cancellationToken)
            );
        }
    }

    /// <summary>
    /// Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    /// as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    /// objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepositoryManager<T> : MongoRepositoryManager<T, string>, IRepositoryManager<T>
        where T : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// Uses the Default App/Web.Config connection strings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connection string.</remarks>
        public MongoRepositoryManager()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connection string to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString)
            : base(connectionString) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public MongoRepositoryManager(MongoRepositoryConfig config)
            : base(config) { }
    }
}
