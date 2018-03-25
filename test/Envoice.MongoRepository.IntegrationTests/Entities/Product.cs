using Envoice.MongoRepository;

namespace Envoice.MongoRepository.IntegrationTests.Entities
{
    /// <summary>
    /// Business Entity for Product
    /// </summary>
    public class Product : Entity
    {
        public Product()
        {
        }

        public Product(string name, string description, decimal price) 
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;      
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}