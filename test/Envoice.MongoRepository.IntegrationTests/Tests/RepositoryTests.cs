using System;
using System.Collections.Generic;
using System.Linq;
using Envoice.MongoRepository.Impl;
using Envoice.MongoRepository.IntegrationTests.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace Envoice.MongoRepository.IntegrationTests.Tests
{
    [Collection(Collections.Database)]
    public class RepositoryTests : TestsBase
    {
        [Fact]
        public void CanAddDocument()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers[0]);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void CanAddMultipleDocuments()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
            this.TestCustomers[1].Id.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async void CanAddDocumentAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers[0]);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async void CanAddMultipleDocumentsAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);
            this.TestCustomers[0].Id.ShouldNotBeNullOrWhiteSpace();
            this.TestCustomers[1].Id.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void CanCountDocuments()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            var count = _customerRepository.Count();
            count.ShouldBe(this.TestCustomers.Count);
        }

        [Fact]
        public async void CanCountDocumentsAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            var count = await _customerRepository.CountAsync();
            count.ShouldBe(this.TestCustomers.Count);
        }

        [Fact]
        public void CanDeleteDocument()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            _customerRepository.Delete(this.TestCustomers[0]);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public async void CanDeleteDocumentAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            await _customerRepository.DeleteAsync(this.TestCustomers[0]);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public void CanDeleteDocumentById()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            _customerRepository.Delete(this.TestCustomers[0].Id);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public async void CanDeleteDocumentByIdAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();
            var count = _customerRepository.Count();
            _customerRepository.Add(this.TestCustomers);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            await _customerRepository.DeleteAsync(this.TestCustomers[0].Id);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public void CanDeleteDocumentByExpression()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            _customerRepository.Delete(x => x.Id == this.TestCustomers[0].Id);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public async void CanDeleteDocumentByExpressionAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            await _customerRepository.DeleteAsync(x => x.Id == this.TestCustomers[0].Id);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count - 1);
        }

        [Fact]
        public void CanDeleteAllDocuments()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            _customerRepository.DeleteAll();
            _customerRepository.Count().ShouldBe(0);
        }

        [Fact]
        public async void CanDeleteAllDocumentsAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            await _customerRepository.DeleteAllAsync();
            _customerRepository.Count().ShouldBe(0);
        }

        [Fact]
        public void CanCheckIfDocumentExists()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            var exists = _customerRepository.Exists(x => x.Id == this.TestCustomers[0].Id);
            exists.ShouldBeTrue();
        }

        [Fact]
        public async void CanCheckIfDocumentExistsAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            var exists = await _customerRepository.ExistsAsync(x => x.Id == this.TestCustomers[0].Id);
            exists.ShouldBeTrue();
        }

        [Fact]
        public void CanGetSingleDocumentById()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            var document = _customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
        }

        [Fact]
        public async void CanGetSingleDocumentByIdAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);
            _customerRepository.Count().ShouldBe(this.TestCustomers.Count);

            var document = await _customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
        }

        [Fact]
        public void CanUpdateDocument()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName";
            _customerRepository.Update(this.TestCustomers[0]);

            var document = _customerRepository.GetById(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
            document.LastName.ShouldBe("Updated LastName");
        }

        [Fact]
        public async void CanUpdateDocumentAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName";
            await _customerRepository.UpdateAsync(this.TestCustomers[0]);

            var document = await _customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(this.TestCustomers[0].Id);
            document.LastName.ShouldBe("Updated LastName");
        }

        [Fact]
        public void CanUpdateMultipleDocuments()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            _customerRepository.Add(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName 1";
            this.TestCustomers[1].LastName = "Updated LastName 2";
            _customerRepository.Update(this.TestCustomers);

            var document1 = _customerRepository.GetById(this.TestCustomers[0].Id);
            document1.ShouldNotBeNull();
            document1.Id.ShouldBe(this.TestCustomers[0].Id);
            document1.LastName.ShouldBe("Updated LastName 1");


            var document2 = _customerRepository.GetById(this.TestCustomers[1].Id);
            document2.ShouldNotBeNull();
            document2.Id.ShouldBe(this.TestCustomers[1].Id);
            document2.LastName.ShouldBe("Updated LastName 2");
        }

        [Fact]
        public async void CanUpdateMultipleDocumentsAsync()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

            await _customerRepository.AddAsync(this.TestCustomers);

            this.TestCustomers[0].LastName = "Updated LastName 1";
            this.TestCustomers[1].LastName = "Updated LastName 2";
            await _customerRepository.UpdateAsync(this.TestCustomers);

            var document1 = await _customerRepository.GetByIdAsync(this.TestCustomers[0].Id);
            document1.ShouldNotBeNull();
            document1.Id.ShouldBe(this.TestCustomers[0].Id);
            document1.LastName.ShouldBe("Updated LastName 1");


            var document2 = await _customerRepository.GetByIdAsync(this.TestCustomers[1].Id);
            document2.ShouldNotBeNull();
            document2.Id.ShouldBe(this.TestCustomers[1].Id);
            document2.LastName.ShouldBe("Updated LastName 2");
        }

        [Fact]
        public void AddAndUpdateTest()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();
            IRepositoryManager<Customer> _customerManager = new MongoRepositoryManager<Customer>();

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
                PostCode = "40990",
                City = "George Town",
                Country = "Alaska"
            };

            _customerRepository.Add(customer);

            _customerManager.Exists.ShouldBeTrue();
            customer.Id.ShouldNotBeNullOrWhiteSpace();

            // fetch it back
            var alreadyAddedCustomer = _customerRepository.Where(c => c.FirstName == "Bob").Single();

            alreadyAddedCustomer.ShouldNotBeNull();
            customer.FirstName.ShouldBe(alreadyAddedCustomer.FirstName);
            customer.HomeAddress.Address1.ShouldBe(alreadyAddedCustomer.HomeAddress.Address1);

            alreadyAddedCustomer.Phone = "10110111";
            alreadyAddedCustomer.Email = new CustomerEmail("dil.bob@fastmail.org");

            _customerRepository.Update(alreadyAddedCustomer);

            // fetch by id now
            var updatedCustomer = _customerRepository.GetById(customer.Id);

            updatedCustomer.ShouldNotBeNull();
            alreadyAddedCustomer.Phone.ShouldBe(updatedCustomer.Phone);
            alreadyAddedCustomer.Email.Value.ShouldBe(updatedCustomer.Email.Value);
            _customerRepository.Exists(c => c.HomeAddress.Country == "Alaska").ShouldBeTrue();
        }

        [Fact]
        public void ComplexEntityTest()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();
            IRepository<Product> _productRepo = new MongoRepository<Product>();

            var customer = new Customer();
            customer.FirstName = "Erik";
            customer.LastName = "Swaun";
            customer.Phone = "123 99 8767";
            customer.Email = new CustomerEmail("erick@mail.com");
            customer.HomeAddress = new Address
            {
                Address1 = "Main bulevard",
                Address2 = "1 west way",
                PostCode = "89560",
                City = "Tempare",
                Country = "Arizona"
            };

            var order = new Order();
            order.PurchaseDate = DateTime.Now.AddDays(-2);
            var orderItems = new List<OrderItem>();

            var shampoo = _productRepo.Add(new Product() { Name = "Palmolive Shampoo", Price = 5 });
            var paste = _productRepo.Add(new Product() { Name = "Mcleans Paste", Price = 4 });


            var item1 = new OrderItem { Product = shampoo, Quantity = 1 };
            var item2 = new OrderItem { Product = paste, Quantity = 2 };

            orderItems.Add(item1);
            orderItems.Add(item2);

            order.Items = orderItems;

            customer.Orders = new List<Order>
            {
                order
            };

            _customerRepository.Add(customer);

            customer.Id.ShouldNotBeNull();
            customer.Orders[0].Items[0].Product.Id.ShouldNotBeNullOrWhiteSpace();

            // get the orders
            var theOrders = _customerRepository.Where(c => c.Id == customer.Id).Select(c => c.Orders).ToList();
            var theOrderItems = theOrders[0].Select(o => o.Items);

            theOrders.ShouldNotBeNull();
            theOrderItems.ShouldNotBeNull();
        }


        [Fact]
        public void BatchTest()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>();

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
            _customerRepository.Add(custlist);

            var count = _customerRepository.Count();
            count.ShouldBe(7);
            foreach (Customer c in custlist)
                c.Id.ShouldNotBe(new string('0', 24));

            //Update batch
            foreach (Customer c in custlist)
                c.LastName = c.FirstName;
            _customerRepository.Update(custlist);

            foreach (Customer c in _customerRepository)
                c.FirstName.ShouldBe(c.LastName);

            //Delete by criteria
            _customerRepository.Delete(f => f.FirstName.StartsWith("Client"));

            count = _customerRepository.Count();
            count.ShouldBe(4);

            //Delete specific object
            _customerRepository.Delete(custlist[0]);

            //Test AsQueryable
            var selectedcustomers = from cust in _customerRepository
                                    where cust.LastName.EndsWith("C") || cust.LastName.EndsWith("G")
                                    select cust;

            selectedcustomers.ToList().Count.ShouldBe(2);

            count = _customerRepository.Count();
            count.ShouldBe(3);

            //Drop entire repo
            new MongoRepositoryManager<Customer>().Drop();

            count = _customerRepository.Count();
            count.ShouldBe(0);
        }

        [Fact]
        public void CollectionNamesTest()
        {
            var a = new MongoRepository<Animal>();
            var am = new MongoRepositoryManager<Animal>();
            var va = new Dog();
            am.Exists.ShouldBeFalse();
            a.Update(va);
            am.Exists.ShouldBeTrue();
            a.GetById(va.Id).ShouldBeOfType(typeof(Dog));
            am.Name.ShouldBe("AnimalsTest");
            a.CollectionName.ShouldBe("AnimalsTest");

            var cl = new MongoRepository<CatLike>();
            var clm = new MongoRepositoryManager<CatLike>();
            var vcl = new Lion();
            clm.Exists.ShouldBeFalse();
            cl.Update(vcl);
            clm.Exists.ShouldBeTrue();
            cl.GetById(vcl.Id).ShouldBeOfType(typeof(Lion));
            clm.Name.ShouldBe("Catlikes");
            cl.CollectionName.ShouldBe("Catlikes");

            var b = new MongoRepository<Bird>();
            var bm = new MongoRepositoryManager<Bird>();
            var vb = new Bird();
            bm.Exists.ShouldBeFalse();
            b.Update(vb);
            bm.Exists.ShouldBeTrue();
            b.GetById(vb.Id).ShouldBeOfType(typeof(Bird));
            bm.Name.ShouldBe("Birds");
            b.CollectionName.ShouldBe("Birds");

            var l = new MongoRepository<Lion>();
            var lm = new MongoRepositoryManager<Lion>();
            var vl = new Lion();
            //Assert.IsFalse(lm.Exists);   //Should already exist (created by cl)
            l.Update(vl);
            lm.Exists.ShouldBeTrue();
            l.GetById(vl.Id).ShouldBeOfType(typeof(Lion));
            lm.Name.ShouldBe("Catlikes");
            l.CollectionName.ShouldBe("Catlikes");

            var d = new MongoRepository<Dog>();
            var dm = new MongoRepositoryManager<Dog>();
            var vd = new Dog();
            //Assert.IsFalse(dm.Exists);
            d.Update(vd);
            dm.Exists.ShouldBeTrue();
            d.GetById(vd.Id).ShouldBeOfType(typeof(Dog));
            dm.Name.ShouldBe("AnimalsTest");
            d.CollectionName.ShouldBe("AnimalsTest");

            var m = new MongoRepository<Bird>();
            var mm = new MongoRepositoryManager<Bird>();
            var vm = new Macaw();
            //Assert.IsFalse(mm.Exists);
            m.Update(vm);
            mm.Exists.ShouldBeTrue();
            m.GetById(vm.Id).ShouldBeOfType(typeof(Macaw));
            mm.Name.ShouldBe("Birds");
            m.CollectionName.ShouldBe("Birds");

            var w = new MongoRepository<Whale>();
            var wm = new MongoRepositoryManager<Whale>();
            var vw = new Whale();
            wm.Exists.ShouldBeFalse();
            w.Update(vw);
            wm.Exists.ShouldBeTrue();
            w.GetById(vw.Id).ShouldBeOfType(typeof(Whale));
            wm.Name.ShouldBe("Whale");
            w.CollectionName.ShouldBe("Whale");
        }

        [Fact]
        public void CustomIDTest()
        {
            var customIdRepository = new MongoRepository<CustomIDEntity>();
            var customIdManager = new MongoRepositoryManager<CustomIDEntity>();

            customIdRepository.Add(new CustomIDEntity() { Id = "aaa" });

            customIdManager.Exists.ShouldBeTrue();
            customIdRepository.GetById("aaa").ShouldBeOfType(typeof(CustomIDEntity));
            customIdRepository.GetById("aaa").Id.ShouldBe("aaa");

            customIdRepository.Delete("aaa");
            customIdRepository.Count().ShouldBe(0);

            var y = new MongoRepository<CustomIDEntityCustomCollection>();
            var ym = new MongoRepositoryManager<CustomIDEntityCustomCollection>();

            y.Add(new CustomIDEntityCustomCollection() { Id = "xyz" });

            ym.Exists.ShouldBeTrue();
            ym.Name.ShouldBe("MyTestCollection");
            y.CollectionName.ShouldBe("MyTestCollection");
            y.GetById("xyz").ShouldBeOfType(typeof(CustomIDEntityCustomCollection));

            y.Delete("xyz");
            y.Count().ShouldBe(0);
        }

        [Fact]
        public void CustomIDTypeTest()
        {
            var xint = new MongoRepository<IntCustomer, int>();
            xint.Add(new IntCustomer() { Id = 1, Name = "Test A" });
            xint.Add(new IntCustomer() { Id = 2, Name = "Test B" });

            var yint = xint.GetById(2);
            yint.Name.ShouldBe("Test B");

            xint.Delete(2);
            xint.Count().ShouldBe(1);
        }

        [Fact]
        public void OverrideCollectionName()
        {
            IRepository<Customer> _customerRepository = new MongoRepository<Customer>(Configuration.Database.ConnectionString, "TestCustomers123");
            _customerRepository.Add(new Customer() { FirstName = "Test" });
            _customerRepository.Single().FirstName.Equals("Test").ShouldBeTrue();
            _customerRepository.Collection.CollectionNamespace.CollectionName.ShouldBe("TestCustomers123");
            ((MongoRepository<Customer>)_customerRepository).CollectionName.ShouldBe("TestCustomers123");

            IRepositoryManager<Customer> _curstomerRepoManager = new MongoRepositoryManager<Customer>(Configuration.Database.ConnectionString, "TestCustomers123");
            _curstomerRepoManager.Name.ShouldBe("TestCustomers123");
        }

        #region [ ResolveCollectionNameFromChildClass ]
        public abstract class BaseItem : IEntity
        {
            public string Id { get; set; }

            public DateTime CreatedOn => DateTime.UtcNow;
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
            var repo = new MongoRepository<ClassA>() {
                new ClassB() { Prop1 = "A", Prop2 = "B" } ,
                new ClassC() { Prop1 = "A", Prop3 = "C" }
            };

            repo.Count().ShouldBe(2);

            repo.OfType<ClassA>().Count().ShouldBe(2);
            repo.OfType<ClassB>().Count().ShouldBe(1);
            repo.OfType<ClassC>().Count().ShouldBe(1);
        }
        #endregion
    }
}
