using System;
using System.Linq.Expressions;
using Envoice.MongoRepository;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Driver;
using Xunit;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    [Collection(Collections.Database)]
    public class RepositoryManagerTests
    {
        [Fact]
        public void CanCreateIndexByExpression()
        {
            // var manager = new MongoRepositoryManager<Customer>();
            // var index = manager.EnsureIndex(o => o.Email.Value, true, true, true);
            // manager.DropIndex(index);
        }

        [Fact]
        public void DuplicateIndexIsIdempotent()
        {
            var manager = new MongoRepositoryManager<Customer>();
            var customerRepository = new MongoRepository<Customer>();

            // customerRepository.Add(new Customer{FirstName = "Joe", LastName = "Smith 1"});
            // var index1 = manager.EnsureIndex(o => o.Email.Value, true, true, true);
            // var index2 = manager.EnsureIndex(o => o.Email.Value, true, true, true);
            // Expression<Func<Customer, string>> exp1 = o => o.Logins.Value;
            // Expression<Func<Customer, string>> exp1 = o => o.Email.Value;
            // manager.EnsureIndex( new [] { o => o.Email.Value }, true, true, true);
            // manager.EnsureIndex(new[] {  "Logins.LoginProvider", "Logins.ProviderKey" }, true, true, true);

            // manager.DropIndex(index1);
            // manager.DropIndex(index2);
        }

        [Fact]
        public void CanCreateIndexByString()
        {
            var manager = new MongoRepositoryManager<Customer>();
            // var index = manager.EnsureIndex("Email", true, true, true);
            // manager.DropIndex(index);
        }
    }
}
