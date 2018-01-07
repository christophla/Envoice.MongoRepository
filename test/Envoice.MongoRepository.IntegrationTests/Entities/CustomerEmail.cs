using System;

namespace Envoice.MongoRepository.IntegrationTests.Entities
{
    public class CustomerEmail
    {
        public CustomerEmail(string email)
        {
            Value = email;
        }

        public string Value { get; set; }

    }
}