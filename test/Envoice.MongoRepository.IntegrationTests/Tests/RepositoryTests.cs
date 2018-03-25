using System;
using System.Collections.Generic;
using System.Linq;
using Envoice.MongoRepository;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    [Collection(Collections.Database)]
    public class RepositoryTests : TestsBase
    {
        public RepositoryTests() : base()
        {
            var customerRepository = new MongoRepository<Customer>();
            customerRepository.DeleteAll();
        }

        [Fact]
        public void CanAddDocument()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers[0]);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();

            customerRepository.Delete(this.TestCustomers[0]);
        }

        [Fact]
        public void CanAddMultipleDocuments()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
            this.TestCustomers[1].Id.ShouldNotBeNullOrWhiteSpace();

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanAddDocumentAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers[0]);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();

            await customerRepository.DeleteAsync(this.TestCustomers[0]);
        }

        [Fact]
        public async void CanAddMultipleDocumentsAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
            this.TestCustomers[1].Id.ShouldNotBeNullOrWhiteSpace();

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanCountDocuments()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            var count = customerRepository.Count();
            count.ShouldBe(this.TestCustomers.Count);

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanCountDocumentsAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            var count = await customerRepository.CountAsync();
            count.ShouldBe(this.TestCustomers.Count);

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanDeleteDocument()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            customerRepository.Delete(this.TestCustomers[0]);

            var document = customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanDeleteDocumentAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            await customerRepository.DeleteAsync(this.TestCustomers[0]);

            var document = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanDeleteDocumentById()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            customerRepository.Delete(this.TestCustomers[0].Id);

            var document = customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanDeleteDocumentByIdAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);
            customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            await customerRepository.DeleteAsync(this.TestCustomers[0].Id);

            var document = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanDeleteDocumentByExpression()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            customerRepository.Delete(x => x.Id == this.TestCustomers[0].Id);

            var document = customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanDeleteDocumentByExpressionAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            await customerRepository.DeleteAsync(x => x.Id == this.TestCustomers[0].Id);

            var document = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldBeNull();

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanDeleteAllDocuments()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            customerRepository.DeleteAll();
            customerRepository.Count().ShouldBe(0);
        }

        [Fact]
        public async void CanDeleteAllDocumentsAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            await customerRepository.DeleteAllAsync();
            customerRepository.Count().ShouldBe(0);
        }

        [Fact]
        public void CanCheckIfDocumentExists()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);
            customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            var exists = customerRepository.Exists(x => x.Id == this.TestCustomers[0].Id);
            exists.ShouldBeTrue();

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanCheckIfDocumentExistsAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            var exists = await customerRepository.ExistsAsync(x => x.Id == this.TestCustomers[0].Id);
            exists.ShouldBeTrue();

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanGetSingleDocumentById()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            var document = customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanGetSingleDocumentByIdAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);
            customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            var document = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanUpdateDocument()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName";
            customerRepository.Update(this.TestCustomers[0]);

            var document = customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
            document.LastName.ShouldBe("Updated LastName");

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanUpdateDocumentAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName";
            await customerRepository.UpdateAsync(this.TestCustomers[0]);

            var document = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
            document.LastName.ShouldBe("Updated LastName");

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void CanUpdateMultipleDocuments()
        {
            var customerRepository = new MongoRepository<Customer>();

            customerRepository.Add(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName 1";
            this.TestCustomers[1].LastName = "Updated LastName 2";
            customerRepository.Update(this.TestCustomers);

            var document1 = customerRepository.GetById(this.TestCustomers[0].Id);
            document1.ShouldNotBeNull();
            document1.Id.ShouldBe(this.TestCustomers[0].Id);
            document1.LastName.ShouldBe("Updated LastName 1");

            var document2 = customerRepository.GetById(this.TestCustomers[1].Id);
            document2.ShouldNotBeNull();
            document2.Id.ShouldBe(this.TestCustomers[1].Id);
            document2.LastName.ShouldBe("Updated LastName 2");

            customerRepository.Delete(this.TestCustomers);
        }

        [Fact]
        public async void CanUpdateMultipleDocumentsAsync()
        {
            var customerRepository = new MongoRepository<Customer>();

            await customerRepository.AddAsync(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName 1";
            this.TestCustomers[1].LastName = "Updated LastName 2";
            await customerRepository.UpdateAsync(this.TestCustomers);

            var document1 = await customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document1.ShouldNotBeNull();
            document1.Id.ShouldBe(this.TestCustomers[0].Id);
            document1.LastName.ShouldBe("Updated LastName 1");

            var document2 = await customerRepository.GetByIdAsync(this.TestCustomers[1].Id);
            document2.ShouldNotBeNull();
            document2.Id.ShouldBe(this.TestCustomers[1].Id);
            document2.LastName.ShouldBe("Updated LastName 2");

            await customerRepository.DeleteAsync(this.TestCustomers);
        }

        [Fact]
        public void AddAndUpdateTest()
        {
            var customerRepository = new MongoRepository<Customer>();
            var customerManager = new MongoRepositoryManager<Customer>();

            //_customerManager.Exists.ShouldBeFalse();

            var customer = new Customer();
            customer.FirstName = "Bob";
            customer.LastName = "Dillon";
            customer.Phone = "0900999899";
            customer.Email = new CustomerEmail("Bob.dil@snailmail.com");
            customer.HomeAddress = new Address
            {
                Address1 = "North kingdom 15 west",
                Address2 = "1 north way",
                City = "George Town",
                Country = "Alaska",
                PostCode = "40990"
            };

            customerRepository.Add(customer);

            customerManager.Exists.ShouldBeTrue();
            customer.Id.ShouldNotBeNullOrWhiteSpace();

            // fetch it back
            var alreadyAddedCustomer = customerRepository.Where(c => c.FirstName == "Bob").Single();

            alreadyAddedCustomer.ShouldNotBeNull();
            customer.FirstName.ShouldBe(alreadyAddedCustomer.FirstName);
            customer.HomeAddress.Address1.ShouldBe(alreadyAddedCustomer.HomeAddress.Address1);

            alreadyAddedCustomer.Phone = "10110111";
            alreadyAddedCustomer.Email = new CustomerEmail("dil.bob@fastmail.org");

            customerRepository.Update(alreadyAddedCustomer);

            // fetch by id now
            var updatedCustomer = customerRepository.GetById(customer.Id);

            updatedCustomer.ShouldNotBeNull();
            alreadyAddedCustomer.Phone.ShouldBe(updatedCustomer.Phone);
            alreadyAddedCustomer.Email.Value.ShouldBe(updatedCustomer.Email.Value);
            customerRepository.Exists(c => c.HomeAddress.Country == "Alaska").ShouldBeTrue();

            customerRepository.Delete(updatedCustomer);

            var exists = customerRepository.GetById(updatedCustomer.Id);
            exists.ShouldBeNull();

            customerRepository.DeleteAll();
        }

        [Fact]
        public void ComplexEntityTest()
        {
            var customerRepository = new MongoRepository<Customer>();
            var productRepository = new MongoRepository<Product>();

            var customer = new Customer();
            customer.FirstName = "Erik";
            customer.LastName = "Swaun";
            customer.Phone = "123 99 8767";
            customer.Email = new CustomerEmail("erick@mail.com");
            customer.HomeAddress = new Address
            {
                Address1 = "Main bulevard",
                Address2 = "1 west way",
                City = "Tempare",
                Country = "Arizona",
                PostCode = "89560"
            };

            var order = new Order();
            order.PurchaseDate = DateTime.Now.AddDays(-2);
            var orderItems = new List<OrderItem>();

            var shampoo = productRepository.Add(new Product() { Name = "Palmolive Shampoo", Price = 5 });
            var paste = productRepository.Add(new Product() { Name = "Mcleans Paste", Price = 4 });


            var item1 = new OrderItem { Product = shampoo, Quantity = 1 };
            var item2 = new OrderItem { Product = paste, Quantity = 2 };

            orderItems.Add(item1);
            orderItems.Add(item2);

            order.Items = orderItems;

            customer.Orders = new List<Order>
            {
                order
            };

            customerRepository.Add(customer);

            customer.Id.ShouldNotBeNull();
            customer.Orders[0].Items[0].Product.Id.ShouldNotBeNullOrWhiteSpace();

            // get the orders
            var theCustomer = customerRepository.Where(c => c.Id == customer.Id).SingleOrDefault();

            var theOrders = customerRepository.Where(c => c.Id == customer.Id).Select(c => c.Orders).ToList();
            theOrders.ShouldNotBeNull();
            theOrders.ShouldNotBeEmpty();

            var theOrderItems = theOrders[0].Select(o => o.Items);
            theOrderItems.ShouldNotBeNull();

            customerRepository.DeleteAll();
            productRepository.DeleteAll();
        }


        [Fact]
        public void BatchTest()
        {
            var customerRepository = new MongoRepository<Customer>();

            var custlist = new List<Customer>(new Customer[] {
                new Customer() { FirstName = "Customer A" },
                new Customer() { FirstName = "Client B" },
                new Customer() { FirstName = "Customer C" },
                new Customer() { FirstName = "Client D" },
                new Customer() { FirstName = "Customer E" },
                new Customer() { FirstName = "Client F" },
                new Customer() { FirstName = "Customer G" },
            });

            //Insert batch
            customerRepository.Add(custlist);

            var count = customerRepository.Count();
            count.ShouldBe(7);
            foreach (Customer c in custlist)
                c.Id.ShouldNotBe(new string('0', 24));

            //Update batch
            foreach (Customer c in custlist)
                c.LastName = c.FirstName;
            customerRepository.Update(custlist);

            foreach (Customer c in customerRepository)
                c.FirstName.ShouldBe(c.LastName);

            //Delete by criteria
            customerRepository.Delete(f => f.FirstName.StartsWith("Client"));

            count = customerRepository.Count();
            count.ShouldBe(4);

            //Delete specific object
            customerRepository.Delete(custlist[0]);

            //Test AsQueryable
            var selectedcustomers = from cust in customerRepository
                                    where cust.LastName.EndsWith("C") || cust.LastName.EndsWith("G")
                                    select cust;

            selectedcustomers.ToList().Count.ShouldBe(2);

            count = customerRepository.Count();
            count.ShouldBe(3);

            //Drop entire repo
            customerRepository.DeleteAll();
            count = customerRepository.Count();
            count.ShouldBe(0);
        }

        [Fact]
        public void CollectionNamesTest()
        {
            // animal
            var a = new MongoRepository<Animal>();
            var am = new MongoRepositoryManager<Animal>();
            a.DeleteAll();

            var va = new Dog();
            am.Exists.ShouldBeFalse();
            a.Update(va);
            am.Exists.ShouldBeTrue();
            a.GetById(va.Id).ShouldBeOfType(typeof(Dog));
            am.Name.ShouldBe("AnimalsTest");
            a.CollectionName.ShouldBe("AnimalsTest");

            // cat
            var cl = new MongoRepository<CatLike>();
            var clm = new MongoRepositoryManager<CatLike>();
            cl.DeleteAll();

            var vcl = new Lion();
            clm.Exists.ShouldBeFalse();
            cl.Update(vcl);
            clm.Exists.ShouldBeTrue();
            cl.GetById(vcl.Id).ShouldBeOfType(typeof(Lion));
            clm.Name.ShouldBe("Catlikes");
            cl.CollectionName.ShouldBe("Catlikes");

            // bird
            var b = new MongoRepository<Bird>();
            var bm = new MongoRepositoryManager<Bird>();
            b.DeleteAll();

            var vb = new Bird();
            bm.Exists.ShouldBeFalse();
            b.Update(vb);
            bm.Exists.ShouldBeTrue();
            b.GetById(vb.Id).ShouldBeOfType(typeof(Bird));
            bm.Name.ShouldBe("Birds");
            b.CollectionName.ShouldBe("Birds");

            // lion
            var l = new MongoRepository<Lion>();
            var lm = new MongoRepositoryManager<Lion>();
            l.DeleteAll();

            var vl = new Lion();
            //Assert.IsFalse(lm.Exists);   //Should already exist (created by cl)
            l.Update(vl);
            lm.Exists.ShouldBeTrue();
            l.GetById(vl.Id).ShouldBeOfType(typeof(Lion));
            lm.Name.ShouldBe("Catlikes");
            l.CollectionName.ShouldBe("Catlikes");

            // dog
            var d = new MongoRepository<Dog>();
            var dm = new MongoRepositoryManager<Dog>();
            d.DeleteAll();

            var vd = new Dog();
            //Assert.IsFalse(dm.Exists);
            d.Update(vd);
            dm.Exists.ShouldBeTrue();
            d.GetById(vd.Id).ShouldBeOfType(typeof(Dog));
            dm.Name.ShouldBe("AnimalsTest");
            d.CollectionName.ShouldBe("AnimalsTest");

            // bird
            var m = new MongoRepository<Bird>();
            var mm = new MongoRepositoryManager<Bird>();
            m.DeleteAll();

            var vm = new Macaw();
            m.Update(vm);
            mm.Exists.ShouldBeTrue();
            m.GetById(vm.Id).ShouldBeOfType(typeof(Macaw));
            mm.Name.ShouldBe("Birds");
            m.CollectionName.ShouldBe("Birds");

            // whale
            var w = new MongoRepository<Whale>();
            var wm = new MongoRepositoryManager<Whale>();
            w.DeleteAll();

            var vw = new Whale();
            wm.Exists.ShouldBeFalse();
            w.Update(vw);
            wm.Exists.ShouldBeTrue();
            w.GetById(vw.Id).ShouldBeOfType(typeof(Whale));
            wm.Name.ShouldBe("Whale");
            w.CollectionName.ShouldBe("Whale");

            // cleanup
            am.Drop();
            clm.Drop();
            bm.Drop();
            lm.Drop();
            dm.Drop();
            mm.Drop();
            wm.Drop();
        }

        [Fact]
        public void CustomIDTest()
        {
            var customIdRepository = new MongoRepository<CustomIDEntity>();
            var customIdManager = new MongoRepositoryManager<CustomIDEntity>();
            customIdRepository.DeleteAll();

            customIdRepository.Add(new CustomIDEntity() { Id = "aaa" });

            customIdManager.Exists.ShouldBeTrue();
            customIdRepository.GetById("aaa").ShouldBeOfType(typeof(CustomIDEntity));
            customIdRepository.GetById("aaa").Id.ShouldBe("aaa");

            customIdRepository.Delete("aaa");
            customIdRepository.Count().ShouldBe(0);

            var y = new MongoRepository<CustomIDEntityCustomCollection>();
            var ym = new MongoRepositoryManager<CustomIDEntityCustomCollection>();
            y.DeleteAll();

            y.Add(new CustomIDEntityCustomCollection() { Id = "xyz" });

            ym.Exists.ShouldBeTrue();
            ym.Name.ShouldBe("MyTestCollection");
            y.CollectionName.ShouldBe("MyTestCollection");
            y.GetById("xyz").ShouldBeOfType(typeof(CustomIDEntityCustomCollection));

            y.Delete("xyz");
            y.Count().ShouldBe(0);

            customIdRepository.DeleteAll();
        }

        [Fact]
        public void CustomIDTypeTest()
        {
            var intRepository = new MongoRepository<IntCustomer, int>();
            intRepository.DeleteAll();

            intRepository.Add(new IntCustomer() { Id = 1, Name = "Test A" });
            intRepository.Add(new IntCustomer() { Id = 2, Name = "Test B" });

            var yint = intRepository.GetById(2);
            yint.Name.ShouldBe("Test B");

            intRepository.Delete(2);
            intRepository.Count().ShouldBe(1);

            intRepository.DeleteAll();
        }

        [Fact]
        public void OverrideCollectionName()
        {
            // var config = new MongoRepositoryConfig() {

            // };
            // IRepository<Customer> customerRepository = new MongoRepository<Customer>(Configuration.Database.ConnectionString, "TestCustomers123");
            // customerRepository.Add(new Customer() { FirstName = "Test" });
            // customerRepository.Single().FirstName.Equals("Test").ShouldBeTrue();
            // customerRepository.Collection.CollectionNamespace.CollectionName.ShouldBe("TestCustomers123");
            // ((MongoRepository<Customer>)customerRepository).CollectionName.ShouldBe("TestCustomers123");

            // IRepositoryManager<Customer> _curstomerRepoManager = new MongoRepositoryManager<Customer>(Configuration.Database.ConnectionString, "TestCustomers123");
            // _curstomerRepoManager.Name.ShouldBe("TestCustomers123");
        }

        #region [ ResolveCollectionNameFromChildClass ]
        public abstract class BaseItem : IEntity
        {
            public string Id { get; set; }

            public DateTime CreatedOn => DateTime.UtcNow;

            public string ObjectTypeId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }

        public abstract class BaseA : BaseItem
        { }

        public class SpecialA : BaseA
        { }

        [Fact]
        public void ResolveCollectionNameFromChildClass()
        {
            var specialRepository = new MongoRepository<SpecialA>();
        }
        #endregion

        #region [ CanCreateHierarchicalCollection ]
        public abstract class ClassA : Entity
        {
            public string Prop1 { get; set; }
        }

        public class ClassB : ClassA
        {
            public string Prop2 { get; set; }
        }

        public class ClassC : ClassA
        {
            public string Prop3 { get; set; }
        }

        [Fact]
        public void CanCreateHierarchicalCollection()
        {
            var repository = new MongoRepository<ClassA>() {
                new ClassB() { Prop1 = "A", Prop2 = "B" } ,
                new ClassC() { Prop1 = "A", Prop3 = "C" }
            };

            repository.Count().ShouldBe(2);

            //repo.OfType<ClassA>().Count().ShouldBe(2);
            repository.OfType<ClassB>().Count().ShouldBe(1);
            repository.OfType<ClassC>().Count().ShouldBe(1);

            repository.DeleteAll();
        }
        #endregion
    }
}
