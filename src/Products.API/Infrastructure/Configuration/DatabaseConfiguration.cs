using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure;

namespace Products.API.Infrastructure.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddProductsDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            const string connectionStringName = "Products";

            services
                .AddDbContext<ProductsContext>(options =>
                    {
                        options
                            .UseSqlServer(configuration.GetConnectionString(connectionStringName),
                                sqlServerOptions =>
                                {
                                    sqlServerOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName()
                                        .Name);
                                });
                    }
                );

            return services;
        }

        /// <summary>
        /// Migrate database for local development
        /// </summary>
        /// <remarks>
        /// It takes several seconds until the SQL Server in docker container
        /// which was just spun up is ready to connect.
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
    }
}