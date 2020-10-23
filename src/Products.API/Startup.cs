using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.API.Infrastructure.Services.ProductService;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Infrastructure;
using Products.Infrastructure.Repositories;

namespace Products.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProductsDbContext(Configuration);
            services.AddControllers();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProductsContext productsContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            productsContext.Database.Migrate();
        }
    }

    internal static class CustomExtensionsMethods
    {
        public static IServiceCollection AddProductsDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<ProductsContext>(options =>
                    {
                        options
                            .UseLazyLoadingProxies()
                            .UseSqlServer(configuration.GetConnectionString("Products"),
                            sqlServerOptions =>
                            {
                                sqlServerOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                            });
                    }
                );
            return services;
        }
    }
}
