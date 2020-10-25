using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Repositories;

namespace Products.Infrastructure.DependencyInjection
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddProductsDatabase(services, configuration);
            services.AddTransient<IProductRepository, ProductRepository>();
            return services;
        }

        /// <summary>
        /// Migrate database for local development
        /// </summary>
        /// <remarks>
        /// It takes several seconds until the SQL Server in docker container
        /// is ready to connect when it is just spun up.
        /// This HACK caters for this.
        /// Use only for local development
        /// </remarks>
        /// <param name="dbContext"></param>
        public static void KeepTryingToMigrateDatabase(this DbContext dbContext)
        {
            const int retryDelayInMs = 3000;
            const int maxRetryCount = 20;
            var tryCount = 0;

            bool CanConnect()
            {
                try
                {
                    return dbContext.Database.CanConnect();
                }
                catch (Exception)
                {
                    if (++tryCount == maxRetryCount + 1) throw;
                    return false;
                }
            }

            // HACK for local development
            // Waits till SQL Server docker container is spun up and ready to connect
            while (!CanConnect())
            {
                Task.Delay(retryDelayInMs).GetAwaiter().GetResult();
            }

            dbContext.Database.Migrate();
        }

        private static void AddProductsDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            const string connectionStringName = "Products";

            services
                .AddDbContext<ProductsContext>(options =>
                    {
                        options
                            .UseSqlServer(configuration.GetConnectionString(connectionStringName),
                                sqlServerOptions =>
                                {
                                    sqlServerOptions.MigrationsAssembly(typeof(ProductsContext).Assembly.FullName);
                                });
                    }
                );
        }
    }
}