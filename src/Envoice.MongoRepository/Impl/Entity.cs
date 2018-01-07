using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Envoice.MongoRepository.Impl
{
  /// <summary>
  /// Abstract Entity for all the BusinessEntities.
  /// </summary>
  [DataContract]
  [Serializable]
  [BsonIgnoreExtraElements(Inherited = true)]
  public abstract class Entity : IEntity<string>
  {
    protected Entity()
    {
      Id = ObjectId.GenerateNewId().ToString();
    }

    [DataMember]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; set; }

    [DataMember]
    public DateTime CreatedOn { get; private set; }
  }
}