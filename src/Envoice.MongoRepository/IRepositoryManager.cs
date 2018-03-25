using MongoDB.Driver;
using System.Collections.Generic;
using System;
using MongoDB.Bson;
using Envoice.MongoRepository;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;

namespace Envoice.MongoRepository
{

    /// <summary>
    /// IRepositoryManager definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IRepositoryManager<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        bool Exists { get; }

        /// <summary>
        /// Gets the name of the collection as Mongo uses.
        /// </summary>
        /// <value>The name of the collection as Mongo uses.</value>
        string Name { get; }

        /// <summary>
        /// Drops the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Drop(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DropAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void DropIndex(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DropIndexAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void DropIndexes(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DropIndexesAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void DropAllIndexes(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DropAllIndexesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The name of the newly created index</returns>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        string EnsureIndex(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The name of the newly created index</returns>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        string EnsureIndex(Expression<Func<T, object>> keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The name of the newly created index</returns>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        Task<string> EnsureIndexAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The name of the newly created index</returns>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        Task<string> EnsureIndexAsync(Expression<Func<T, object>> keyname, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(string keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(Expression<Func<T, object>> keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(string keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(Expression<Func<T, object>> keyname, bool descending, bool unique, bool sparse, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(IEnumerable<Expression<Func<T, object>>> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <remarks>
        /// This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(IEnumerable<Expression<Func<T, object>>> keynames, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(IEnumerable<string> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        string EnsureIndex(IEnumerable<Expression<Func<T, object>>> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(IEnumerable<string> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <returns>The name of the newly created index</returns>
        Task<string> EnsureIndexAsync(IEnumerable<Expression<Func<T, object>>> keynames, bool descending = false, bool unique = false, bool sparse = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets stats for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a CollectionStatsResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        CollectionStatsResult GetStats(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns the indexes for this repository.</returns>
        List<BsonDocument> GetIndexes(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns the indexes for this repository.</returns>
        Task<List<BsonDocument>> GetIndexesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        long GetTotalDataSize(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        Task<long> GetTotalDataSizeAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        long GetTotalStorageSize(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        Task<long> GetTotalStorageSizeAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        bool IndexExists(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        Task<bool> IndexExistsAsync(string keyname, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        bool IndexesExists(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        Task<bool> IndexesExistsAsync(IEnumerable<string> keynames, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        bool IsCapped(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        Task<bool> IsCappedAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Runs the ReIndex command on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void ReIndex(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Runs the ReIndex command on this repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task ReIndexAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validates the integrity of the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        ValidateCollectionResult Validate(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validates the integrity of the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        Task<ValidateCollectionResult> ValidateAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// IRepositoryManager definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IRepositoryManager<T> : IRepositoryManager<T, string>
        where T : IEntity<string>
    { }
}
