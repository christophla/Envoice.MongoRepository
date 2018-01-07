using System;
using Envoice.MongoRepository.Impl;
using MongoDB.Bson.Serialization.Attributes;

namespace Envoice.MongoRepository
{
  /// <summary>
  /// Generic Entity interface.
  /// </summary>
  /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
  public interface IEntity<TKey>
  {
    /// <summary>
    /// Gets or sets the Id of the Entity.
    /// </summary>
    /// <value>Id of the Entity.</value>
    [BsonId]
    TKey Id { get; set; }

    /// <summary>
    /// The date the entity was created
    /// </summary>
    DateTime CreatedOn { get; }
  }

  /// <summary>
  /// "Default" Entity interface.
  /// </summary>
  /// <remarks>Entities are assumed to use strings for Id's.</remarks>
  public interface IEntity : IEntity<string>
  {
  }
}