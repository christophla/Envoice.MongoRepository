using Envoice.MongoRepository.Impl;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Driver;
using Xunit;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    [Collection(Collections.Database)]
    public class RepositoryManagerTests : TestsBase
    {
        [Fact]
        public void CanCreateIndexByExpression()
        {
            var manager = new MongoRepositoryManager<Customer>();
            var index = manager.EnsureIndex(o => o.Email.Value, true, true, true);
            manager.DropIndex(index);
        }

        [Fact]
        public void CanCreateIndexByString()
        {
            var manager = new MongoRepositoryManager<Customer>();
            var index = manager.EnsureIndex("Email", true, true, true);
            manager.DropIndex(index);
        }
    }
}
