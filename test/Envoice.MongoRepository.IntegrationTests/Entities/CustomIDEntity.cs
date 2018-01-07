using System;
using Envoice.MongoRepository.Impl;

namespace Envoice.MongoRepository.IntegrationTests.Entities
{
  public class CustomIDEntity : IEntity
  {
    private string _id;
    public string Id
    {
      get { return _id; }
      set { _id = value; }
    }
    public DateTime CreatedOn => DateTime.UtcNow;
  }

  [CollectionName("MyTestCollection")]
  public class CustomIDEntityCustomCollection : CustomIDEntity { }
}