using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Envoice.MongoRepository.Helpers;
using System.Threading;
using Envoice.Conditions;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Implementation for the mongo asynchronous repository
    /// </summary>
    public class MongoRepository<T, TKey> : IRepository<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        protected internal IMongoCollection<T> collection;

        /// <summary>
        /// The repository configuration.
        /// </summary>
        public MongoRepositoryConfig Config { get; }

        /// <summary>
        /// Initializes the mongo bson mappings
        /// </summary>
        static MongoRepository()
        {
            MongoConfig.EnsureConfigured();
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class using the deafult connection string.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepository() : this(Configuration.Database.ConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString) : this(new MongoRepositoryConfig(connectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// /// <param name="config">The configuration.</param>
        public MongoRepository(MongoRepositoryConfig config)
        {
            Condition.Requires(config, "config").IsNotNull();

            this.collection = config.GetCollection<T, TKey>();
            this.Config = config;
        }

        public IMongoCollection<T> Collection
        {
            get { return this.collection; }
        }

        /// <summary>
        /// Gets the name of the collection
        /// </summary>
        public string CollectionName
        {
            get { return this.collection.CollectionNamespace.CollectionName; }
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual T Add(T entity, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.InsertOne(entity, options, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Add(IEnumerable<T> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.InsertMany(entities, options, cancellationToken);
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public async virtual Task<T> AddAsync(T entity, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.InsertOneAsync(entity, options, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        /// <param name="options">Optional insert options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task AddAsync(IEnumerable<T> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.InsertManyAsync(entities, options, cancellationToken);
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <param name="options">Optional count options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count(CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.collection.Count(new BsonDocument(), options, cancellationToken);
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <param name="options">Optional count options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>Count of entities in the collection.</returns>
        public async virtual Task<long> CountAsync(CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.collection.CountAsync(new BsonDocument(), options, cancellationToken);
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {

            this.collection.DeleteOne(GetIDFilter(id), cancellationToken);
        }

        /// <summary>
        /// Deletes an entity from the repository by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the entity.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Delete(ObjectId id, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.DeleteOne(GetIDFilter(id), cancellationToken);
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Delete(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Delete(entity.Id, cancellationToken);
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.DeleteMany<T>(predicate, cancellationToken);
        }

        /// <summary>
        /// Deletes the given entities.
        /// </summary>
        /// <param name="entity">The entities to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Delete(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ids = new List<ObjectId>();
            foreach (var entity in entities)
            {
                ids.Add(new ObjectId(entity.Id as string));
            }
            var filter = Builders<T>.Filter.In("_id", ids);
            this.collection.DeleteMany(filter, cancellationToken);
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.DeleteOneAsync(GetIDFilter(id), cancellationToken);
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.DeleteAsync(entity.Id, cancellationToken);
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.DeleteManyAsync<T>(predicate, cancellationToken);
        }

        /// <summary>
        /// Deletes the given entities.
        /// </summary>
        /// <param name="entity">The entities to delete.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ids = new List<ObjectId>();
            foreach (var entity in entities)
            {
                ids.Add(new ObjectId(entity.Id as string));
            }
            var filter = Builders<T>.Filter.In("_id", ids);
            await this.collection.DeleteManyAsync(filter, cancellationToken);
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void DeleteAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.collection.DeleteMany<T>(t => true, cancellationToken);
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.collection.DeleteManyAsync<T>(t => true, cancellationToken);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            var foo = this.collection.AsQueryable<T>();
            return this.collection.AsQueryable<T>().Any(predicate);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public async virtual Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.collection.AsQueryable().AnyAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <param name="options">Optional find options</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The Entity T.</returns>
        public virtual T GetById(TKey id, FindOptions<T> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.collection.FindSync<T>(GetIDFilter(id), options, cancellationToken).SingleOrDefault();
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <param name="options">Optional find options</param>
        /// <returns>The Entity T.</returns>
        public async virtual Task<T> GetByIdAsync(TKey id, FindOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.collection.Find<T>(GetIDFilter(id), options).SingleOrDefaultAsync<T>(cancellationToken);
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity.Id == null)
                this.Add(entity, null, cancellationToken);
            else
                this.collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true }, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        /// <returns>The updated entity.</returns>
        public async virtual Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity.Id == null)
                await this.AddAsync(entity, null, cancellationToken);
            else
                await this.collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true }, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public virtual void Update(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (T entity in entities)
                this.collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true }, cancellationToken);
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken">Optional threading cancellation token</param>
        public async virtual Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (T entity in entities)
                await this.collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true }, cancellationToken);
        }

        #region [ IQueryable<T> ]

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            var enumerator = this.collection.AsQueryable<T>().GetEnumerator();


            return this.collection.AsQueryable<T>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQueryable is executed.
        /// </summary>
        public virtual Type ElementType
        {
            get { return this.collection.AsQueryable<T>().ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of IQueryable.
        /// </summary>
        public virtual Expression Expression
        {
            get { return this.collection.AsQueryable<T>().Expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public virtual IQueryProvider Provider
        {
            get { return this.collection.AsQueryable<T>().Provider; }
        }

        #endregion

        #region [ Private Methods ]

        private static FilterDefinition<T> GetIDFilter(ObjectId id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }

        private static FilterDefinition<T> GetIDFilter(TKey id)
        {
            if (typeof(T).IsSubclassOf(typeof(Entity)))
                return GetIDFilter(new ObjectId(id as string));
            return Builders<T>.Filter.Eq("_id", id);
        }

        #endregion
    }


    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepository<T> : MongoRepository<T, string>, IRepository<T>
        where T : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepository()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="database">The database isntance.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        // public MongoRepository(IMongoDatabase database, string collectionName)
        //     : base(database, collectionName) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        // public MongoRepository(MongoUrl url)
        //     : base(url) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        // public MongoRepository(MongoUrl url, string collectionName)
        //     : base(url, collectionName) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString)
            : base(connectionString) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="config">The configuration.</param>
        public MongoRepository(MongoRepositoryConfig config)
            : base(config) { }
    }
}
