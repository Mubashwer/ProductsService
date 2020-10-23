using Microsoft.EntityFrameworkCore;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Domain.Common;
using Products.Infrastructure.EntityConfigurations;

namespace Products.Infrastructure
{
    public class ProductsContext : DbContext, IUnitOfWork
    {
        public const string DefaultSchema = "products";

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOptionEntityTypeConfiguration());
        }
    }
}
