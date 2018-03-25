using System;
using Envoice.MongoRepository;

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

        public string ObjectTypeId { get; set; }
    }

    [CollectionName("MyTestCollection")]
    public class CustomIDEntityCustomCollection : CustomIDEntity { }
}
