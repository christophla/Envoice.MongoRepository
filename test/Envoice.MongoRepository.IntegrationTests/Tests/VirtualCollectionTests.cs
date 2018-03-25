using Envoice.MongoRepository;
using Envoice.MongoRepository.IntegrationTests.Entities;
using Shouldly;
using Xunit;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    [Collection(Collections.Database)]
    public class VirtualCollectionTests : TestsBase
    {
        [Fact]
        public void CollectionNameAttributeIsUsed()
        {
            var productRepository = new MongoRepository<VirtualProduct>(this.VirtualConnectionString);
            var productManager = new MongoRepositoryManager<VirtualProduct>(this.VirtualConnectionString);

            var product = new VirtualProduct("TEST_1");

            productRepository.Update(product);
            productManager.Exists.ShouldBeTrue();
            productRepository.GetById(product.Id).ShouldBeOfType(typeof(VirtualProduct));
            productManager.Name.ShouldBe(VirtualCollections.Entities);
            productRepository.CollectionName.ShouldBe(VirtualCollections.Entities);

            productRepository.DeleteAll();
        }

        [Fact]
        public void CollectionNameShouldBeSet()
        {
            var connectionString = "mongodb://localhost/mongo-async-repo?virtual=true";
            var repository = new MongoRepository<VirtualProduct>(connectionString);

            repository.CollectionName.ShouldBe(VirtualCollections.Entities);
        }

        [Fact]
        public void CollectionNameShouldNotBeSetWhenDisabled()
        {
            var connectionString = "mongodb://localhost/database?virtual=false";
            var repository = new MongoRepository<VirtualProduct>(connectionString);

            repository.CollectionName.ShouldBe("VirtualProduct");
        }

        [Fact]
        public void DefaultCollectionNameShouldBeSet()
        {
            var connectionString = "mongodb://localhost/database?virtual=true&virtualCollection=test";
            var repository = new MongoRepository<Product>(connectionString);

            repository.CollectionName.ShouldBe("test");
        }

        [Fact]
        public void DefaultCollectionNameShouldNotBeSetWhenDisabled()
        {
            var connectionString = "mongodb://localhost/database?virtual=false&virtualCollection=test";
            var repository = new MongoRepository<Product>(connectionString);

            repository.CollectionName.ShouldBe("Product");
        }

        [Fact]
        public void CollectionsShouldFilterByObject()
        {
            var products = new MongoRepository<VirtualProduct>(this.VirtualConnectionString);
            var users = new MongoRepository<VirtualUser>(this.VirtualConnectionString);

            products.Add(new VirtualProduct("TEST_PRODUCT_1"));
            users.Add(new VirtualUser("TEST_PRODUCT_1"));

            products.Count().ShouldBe(1);
            users.Count().ShouldBe(1);

            products.DeleteAll();
            users.DeleteAll();
        }

        [Fact]
        public void DeleteAllShouldFilterByObject()
        {

            var products = new MongoRepository<VirtualProduct>(this.VirtualConnectionString);
            var users = new MongoRepository<VirtualUser>(this.VirtualConnectionString);

            var product = new VirtualProduct("TEST_PRODUCT_1");
            products.Add(product);

            var user = new VirtualUser("TEST_PRODUCT_1");
            users.Add(user);

            products.DeleteAll();

            var existingProduct = products.GetById(product.Id);
            existingProduct.ShouldBeNull();

            var existingUser = users.GetById(user.Id);
            existingUser.ShouldNotBeNull();

            products.DeleteAll();
            users.DeleteAll();
        }
    }
}
