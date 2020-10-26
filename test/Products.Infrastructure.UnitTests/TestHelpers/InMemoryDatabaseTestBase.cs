using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Persistence;

namespace Products.Infrastructure.UnitTests.TestHelpers
{
    public abstract class InMemoryDatabaseTestBase : IDisposable
    {
        private readonly IList<SqliteConnection> _databaseConnections = new List<SqliteConnection>();

        protected ProductsContext GetProductsContext()
        {
            // creating an in-memory sqlite database with a unique name
            var connectionString = $"DataSource={Guid.NewGuid()};mode=memory;cache=shared";
            var connection = new SqliteConnection(connectionString);

            // sqlite connections need to be open manually for it to work
            connection.Open();

            _databaseConnections.Add(connection);

            var options = new DbContextOptionsBuilder<ProductsContext>()
                .UseSqlite(connection)
                .Options;

            var productContext = new ProductsContext(options);
            productContext.Database.Migrate();

            return productContext;
        }

        public void Dispose()
        {
            foreach (var connection in _databaseConnections)
            {
                connection.Close();
            }
        }
    }
}
