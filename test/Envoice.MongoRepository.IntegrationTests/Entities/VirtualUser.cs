using Envoice.MongoRepository;

namespace Envoice.MongoRepository.IntegrationTests.Entities
{
    [VirtualCollectionName(VirtualCollections.Entities)]
    public class VirtualUser : Entity
    {
        public string Name { get; set; }

        public VirtualUser(string name)
        {
            this.Name = name;
        }
    }
}
