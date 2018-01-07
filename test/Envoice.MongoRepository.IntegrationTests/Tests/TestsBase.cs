using System;
using System.Collections.Generic;
using Envoice.MongoRepository.Impl;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Driver;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    /// <summary>
    /// Setup and teardown of database
    /// </summary>
    public abstract class TestsBase
    {
        protected IList<Customer> TestCustomers;

        protected TestsBase() : base()
        {
            var url = new MongoUrl(Configuration.Database.ConnectionString);
            var client = new MongoClient(url);
            client.DropDatabase(url.DatabaseName);

            InitializeTestData();
        }

        /// <summary>
        /// Initializes the test data
        /// </summary>
        protected void InitializeTestData()
        {
            this.TestCustomers = new List<Customer>{
                new Customer{FirstName = "Joe", LastName = "Smith 1"},
                new Customer{FirstName = "Joe", LastName = "Smith 2"},
                new Customer{FirstName = "Joe", LastName = "Smith 3"}
            };
        }
    }
}