using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Envoice.MongoRepository
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    [DataContract]
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
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        /// <summary>
        /// The object type id.
        /// </summary>
        /// <remarks>
        /// Used for virtual collections.
        /// </remarks>
        /// <returns></returns>
        [DataMember]
        public virtual string ObjectTypeId { get; set; }

        /// <summary>
        /// The date the object was created.
        /// </summary>
        [DataMember]
        public DateTime CreatedOn { get; private set; }
    }
}
