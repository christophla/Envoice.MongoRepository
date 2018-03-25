using Envoice.MongoRepository;

namespace Envoice.MongoRepository.IntegrationTests.Entities
{
    [VirtualCollectionName(VirtualCollections.Entities)]
    public class VirtualProduct : Entity
    {
        public string Name { get; set; }

        public VirtualProduct(string name)
        {
            this.Name = name;
        }
    }
}
