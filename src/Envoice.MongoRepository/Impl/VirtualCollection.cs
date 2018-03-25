
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Envoice.Conditions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Envoice.MongoRepository
{
    public class VirtualCollection<T, TKey> : IMongoCollection<T> where T : IEntity<TKey>
    {
        protected IMongoCollection<T> MongoCollection;
        protected MongoRepositoryConfig Config;

        public VirtualCollection(IMongoCollection<T> mongoCollection, MongoRepositoryConfig config)
        {
            Condition.Requires(config, "config").IsNotNull();
            Condition.Requires(mongoCollection, "mongoCollection").IsNotNull();
            this.Config = config;
            this.MongoCollection = mongoCollection;
        }

        public CollectionNamespace CollectionNamespace => MongoCollection.CollectionNamespace;

        public IMongoDatabase Database => MongoCollection.Database;

        public IBsonSerializer<T> DocumentSerializer => MongoCollection.DocumentSerializer;

        public IMongoIndexManager<T> Indexes => MongoCollection.Indexes;

        public MongoCollectionSettings Settings => MongoCollection.Settings;

        public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<T, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Aggregate(ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session, PipelineDefinition<T, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Aggregate(session, ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<T, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.AggregateAsync(ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session, PipelineDefinition<T, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.AggregateAsync(session, ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public BulkWriteResult<T> BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public BulkWriteResult<T> BulkWrite(IClientSessionHandle session, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<BulkWriteResult<T>> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<BulkWriteResult<T>> BulkWriteAsync(IClientSessionHandle session, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public long Count(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Count(ApplyFilters(filter), options, cancellationToken);
        }

        public long Count(IClientSessionHandle session, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Count(session, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<long> CountAsync(FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.CountAsync(ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<long> CountAsync(IClientSessionHandle session, FilterDefinition<T> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.CountAsync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public DeleteResult DeleteMany(FilterDefinition<T> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteMany(ApplyFilters(filter), cancellationToken);
        }

        public DeleteResult DeleteMany(FilterDefinition<T> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteMany(ApplyFilters(filter), options, cancellationToken);
        }

        public DeleteResult DeleteMany(IClientSessionHandle session, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteMany(session, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteManyAsync(ApplyFilters(filter), cancellationToken);
        }

        public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteManyAsync(ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteManyAsync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public DeleteResult DeleteOne(FilterDefinition<T> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteOne(ApplyFilters(filter), cancellationToken);
        }

        public DeleteResult DeleteOne(FilterDefinition<T> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteOne(ApplyFilters(filter), options, cancellationToken);
        }

        public DeleteResult DeleteOne(IClientSessionHandle session, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.DeleteOne(session, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteOneAsync(ApplyFilters(filter), cancellationToken);
        }

        public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteOneAsync(ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DeleteOneAsync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public IAsyncCursor<TField> Distinct<TField>(FieldDefinition<T, TField> field, FilterDefinition<T> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Distinct(field, ApplyFilters(filter), options, cancellationToken);
        }

        public IAsyncCursor<TField> Distinct<TField>(IClientSessionHandle session, FieldDefinition<T, TField> field, FilterDefinition<T> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Distinct(session, field, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<T, TField> field, FilterDefinition<T> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DistinctAsync(field, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TField>> DistinctAsync<TField>(IClientSessionHandle session, FieldDefinition<T, TField> field, FilterDefinition<T> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.DistinctAsync(session, field, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<T> filter, FindOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindAsync(ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, FindOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindAsync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public TProjection FindOneAndDelete<TProjection>(FilterDefinition<T> filter, FindOneAndDeleteOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndDelete(ApplyFilters(filter), options, cancellationToken);
        }

        public TProjection FindOneAndDelete<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, FindOneAndDeleteOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndDelete(session, ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndDeleteAsync<TProjection>(FilterDefinition<T> filter, FindOneAndDeleteOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndDeleteAsync(ApplyFilters(filter), options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndDeleteAsync<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, FindOneAndDeleteOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndDeleteAsync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public TProjection FindOneAndReplace<TProjection>(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndReplace(ApplyFilters(filter), replacement, options, cancellationToken);
        }

        public TProjection FindOneAndReplace<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndReplace(session, ApplyFilters(filter), replacement, options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndReplaceAsync<TProjection>(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndReplaceAsync(ApplyFilters(filter), replacement, options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndReplaceAsync<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndReplaceAsync(session, ApplyFilters(filter), replacement, options, cancellationToken);
        }

        public TProjection FindOneAndUpdate<TProjection>(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndUpdate(ApplyFilters(filter), update, options, cancellationToken);
        }

        public TProjection FindOneAndUpdate<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindOneAndUpdate(session, ApplyFilters(filter), update, options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndUpdateAsync<TProjection>(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndUpdateAsync(ApplyFilters(filter), update, options, cancellationToken);
        }

        public async Task<TProjection> FindOneAndUpdateAsync<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.FindOneAndUpdateAsync(session, ApplyFilters(filter), update, options, cancellationToken);
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(FilterDefinition<T> filter, FindOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindSync(ApplyFilters(filter), options, cancellationToken);
        }

        public IAsyncCursor<TProjection> FindSync<TProjection>(IClientSessionHandle session, FilterDefinition<T> filter, FindOptions<T, TProjection> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.FindSync(session, ApplyFilters(filter), options, cancellationToken);
        }

        public void InsertMany(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MongoCollection.InsertMany(ApplyProperties(documents), options, cancellationToken);
        }

        public void InsertMany(IClientSessionHandle session, IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MongoCollection.InsertMany(session, ApplyProperties(documents), options, cancellationToken);
        }

        public async Task InsertManyAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.MongoCollection.InsertManyAsync(ApplyProperties(documents), options, cancellationToken);
        }

        public async Task InsertManyAsync(IClientSessionHandle session, IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.MongoCollection.InsertManyAsync(session, ApplyProperties(documents), options, cancellationToken);
        }

        public void InsertOne(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MongoCollection.InsertOne(ApplyProperties(document), options, cancellationToken);
        }

        public void InsertOne(IClientSessionHandle session, T document, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MongoCollection.InsertOne(session, ApplyProperties(document), options, cancellationToken);
        }

        public async Task InsertOneAsync(T document, CancellationToken cancellationToken)
        {
            var options = new InsertOneOptions(); //TODO: Make this configurable at runtime
            await this.MongoCollection.InsertOneAsync(ApplyProperties(document), options, cancellationToken);
        }

        public async Task InsertOneAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.MongoCollection.InsertOneAsync(ApplyProperties(document), options, cancellationToken);
        }

        public async Task InsertOneAsync(IClientSessionHandle session, T document, InsertOneOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.MongoCollection.InsertOneAsync(session, ApplyProperties(document), options, cancellationToken);
        }

        public IAsyncCursor<TResult> MapReduce<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<T, TResult> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public IAsyncCursor<TResult> MapReduce<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<T, TResult> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<T, TResult> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(IClientSessionHandle session, BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<T, TResult> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public IFilteredMongoCollection<TDerivedDocument> OfType<TDerivedDocument>() where TDerivedDocument : T
        {
            throw new System.NotImplementedException();
        }

        public ReplaceOneResult ReplaceOne(FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.ReplaceOne(ApplyFilters(filter), ApplyProperties(replacement), options, cancellationToken);
        }

        public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.ReplaceOne(session, ApplyFilters(filter), ApplyProperties(replacement), options, cancellationToken);
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.ReplaceOneAsync(ApplyFilters(filter), ApplyProperties(replacement), options, cancellationToken);
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<T> filter, T replacement, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.ReplaceOneAsync(session, ApplyFilters(filter), ApplyProperties(replacement), options, cancellationToken);
        }

        public UpdateResult UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateMany(ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public UpdateResult UpdateMany(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateMany(session, ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateManyAsync(ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateManyAsync(session, ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public UpdateResult UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateOne(ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public UpdateResult UpdateOne(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.UpdateOne(session, ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.UpdateOneAsync(ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public async Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.UpdateOneAsync(session, ApplyFilters(filter), ApplyProperties(update), options, cancellationToken);
        }

        public IAsyncCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<T>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Watch(ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public IAsyncCursor<TResult> Watch<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<T>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.MongoCollection.Watch(session, ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<T>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.WatchAsync(ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public async Task<IAsyncCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<T>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MongoCollection.WatchAsync(session, ApplyObjectTypePipelineMatch<TResult>(pipeline), options, cancellationToken);
        }

        public IMongoCollection<T> WithReadConcern(ReadConcern readConcern)
        {
            var mongoCollection = this.MongoCollection.WithReadConcern(readConcern);
            return new VirtualCollection<T, TKey>(mongoCollection, this.Config);
        }

        public IMongoCollection<T> WithReadPreference(ReadPreference readPreference)
        {
            var mongoCollection = this.MongoCollection.WithReadPreference(readPreference);
            return new VirtualCollection<T, TKey>(mongoCollection, this.Config);
        }

        public IMongoCollection<T> WithWriteConcern(WriteConcern writeConcern)
        {
            var mongoCollection = this.MongoCollection.WithWriteConcern(writeConcern);
            return new VirtualCollection<T, TKey>(mongoCollection, this.Config);
        }

        #region [ Private Methods ]

        private FilterDefinition<T> ApplyFilters(FilterDefinition<T> filter)
        {
            var builder = Builders<T>.Filter;
            return filter & builder.Eq(EntityFields.ObjectTypeId, GetTypeName(typeof(T)));
        }

        private T ApplyProperties(T item)
        {
            item.ObjectTypeId = GetTypeName(typeof(T));
            return item;
        }

        private IEnumerable<T> ApplyProperties(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                ApplyProperties(item);
            }
            return items;
        }

        private UpdateDefinition<T> ApplyProperties(UpdateDefinition<T> updateDefinition)
        {
            var builder = Builders<T>.Update;
            return Builders<T>.Update.Combine(updateDefinition, builder.Set(o => o.ObjectTypeId, GetTypeName(typeof(T))));
        }

        private PipelineDefinition<T, TResult> ApplyObjectTypePipelineMatch<TResult>(PipelineDefinition<T, TResult> pipeline)
        {
            var builder = Builders<TResult>.Filter;
            var filter = builder.Eq(EntityFields.ObjectTypeId, GetTypeName(typeof(T)));

            return pipeline.Match(filter);
        }

        private PipelineDefinition<ChangeStreamDocument<T>, TResult> ApplyObjectTypePipelineMatch<TResult>(PipelineDefinition<ChangeStreamDocument<T>, TResult> pipeline)
        {
            var builder = Builders<TResult>.Filter;
            var filter = builder.Eq(EntityFields.ObjectTypeId, GetTypeName(typeof(T)));

            return pipeline.Match(filter);
        }

        private string GetTypeName(Type type)
        {
            var virtualCollectionTypeNameAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(VirtualCollectionTypeName));
            if (null != virtualCollectionTypeNameAttribute)
            {
                return ((VirtualCollectionTypeName)virtualCollectionTypeNameAttribute).TypeName;
            }
            else
            {
                return type.Name;
            }
        }

        #endregion
    }
}
