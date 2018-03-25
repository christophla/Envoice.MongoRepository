using System;
using System.Collections.Generic;
using Bogus;
using Envoice.MongoRepository;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    /// <summary>
    /// Setup and teardown of database
    /// </summary>
    public abstract class TestsBase
    {
        protected IList<Customer> TestCustomers;
        protected string ConnectionString => Configuration.Database.ConnectionString;
        protected string VirtualConnectionString => Configuration.Database.VirtualConnectionString;

        protected TestsBase() : base()
        {
            // var url = new MongoUrl(Configuration.Database.ConnectionString);
            // var client = new MongoClient(url);
            // var database = client.GetDatabase(url.DatabaseName);

            // // clear collections per test
            // foreach (var item in database.ListCollections().ToList<BsonDocument>())
            // {
            //     database.DropCollection(item.ToString());
            // }

            // var customerRepository = new MongoRepository<Customer>();
            // customerRepository.DeleteAll();

            var faker = new Faker("en");
            this.TestCustomers = new List<Customer>{
                new Customer{FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName()},
                new Customer{FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName()},
                new Customer{FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName()}
            };
        }

    }
}
