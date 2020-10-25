using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Persistence;

namespace Products.API.IntegrationTests.TestHelpers
{
    public class TestApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IList<SqliteConnection> _databaseConnections = new List<SqliteConnection>();

        /// <summary>
        /// Sets up an in-memory database before returning the client
        /// </summary>
        /// <param name="configureContext">use this to seed the database</param>
        /// <returns>the <see cref="HttpClient"/> to make requests against the API</returns>
        public HttpClient CreateClientWithInMemoryDb(Action<ProductsContext> configureContext = default!)
        {
            return WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    x => x.ServiceType ==
                         typeof(DbContextOptions<ProductsContext>));

                services.Remove(descriptor);

                // creating an in-memory sqlite database with a unique name
                var connectionString = $"DataSource={Guid.NewGuid()};mode=memory;cache=shared";
                var connection = new SqliteConnection(connectionString);

                // sqlite connections need to be open manually for it to work
                connection.Open();
                _databaseConnections.Add(connection);

                services.AddDbContext<ProductsContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedServices = scope.ServiceProvider;

                var productsContext = scopedServices.GetRequiredService<ProductsContext>();
                productsContext.Database.Migrate();
                configureContext?.Invoke(productsContext);

            })).CreateClient();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var connection in _databaseConnections)
                {
                    connection.Close();
                }
            }
            base.Dispose(disposing);
        }
    }
}
