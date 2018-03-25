using System;
using Microsoft.Extensions.Configuration;

namespace Envoice.MongoRepository.IntegrationTests
{
    /// <summary>
    /// Configuration for the library.
    /// </summary>
    public static class Configuration
    {
        private static IConfigurationRoot _configuration;

        static Configuration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();
        }

        /// <summary>
        /// Database settings
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// The database connection string
            /// </summary>
            public static string ConnectionString
            {
                get { return _configuration.GetConnectionString("Mongo"); }
            }

            /// <summary>
            /// The database connection string
            /// </summary>
            public static string VirtualConnectionString
            {
                get { return _configuration.GetConnectionString("MongoVirtual"); }
            }

        }
    }
}
