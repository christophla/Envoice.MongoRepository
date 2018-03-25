using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IRepository<T, TKey> : IQueryable<T> where T : IEntity<TKey>
    {
        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        IMongoCollection<T> Collection { get; }

        /// <summary>
        /// The mongo collection name
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        T Add(T entity, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        Task<T> AddAsync(T entity, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Add(IEnumerable<T> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task AddAsync(IEnumerable<T> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <param name="options">Optional count options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Count of entities in the repository.</returns>
        long Count(CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <param name="options">Optional count options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Count of entities in the repository.</returns>
        Task<long> CountAsync(CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Delete(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the provided entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Delete(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the provided entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void DeleteAll(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        bool Exists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <param name="options">Optional find options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The Entity T.</returns>
        T GetById(TKey id, FindOptions<T> options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <param name="options">Optional find options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The Entity T.</returns>
        Task<T> GetByIdAsync(TKey id, FindOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        void Update(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The updated entity.</returns>
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IRepository<T> : IQueryable<T>, IRepository<T, string>
        where T : IEntity<string>
    { }
}
