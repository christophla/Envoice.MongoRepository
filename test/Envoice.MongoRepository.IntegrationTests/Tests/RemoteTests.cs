// using Envoice.MongoRepository.IntegrationTests.Entities;
// using Shouldly;
// using Xunit;

// namespace Envoice.MongoRepository.IntegrationTests.Tests
// {
//     public class RemoteTests
//     {
//         [Fact]
//         public void CanConnect()
//         {

//             IRepository<Customer> customerRepository = new MongoRepository<Customer>();
//             var customer = new Customer { FirstName = "Joe", LastName = "Smith 1" };
//             customerRepository.Add(customer);

//             customerRepository.Delete(customer);

//             var customer2 = customerRepository.GetById(customer.Id);

//             customer2.ShouldBeNull();

//         }
//     }
// }
