using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity<string>
    {
        /// <summary>
        /// Internal constructor
        /// </summary>
        protected Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        /// <summary>
        /// The object id
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}
