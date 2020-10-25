using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.Infrastructure.Persistence.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("TEXT")
                .IsRequired(false);

            builder.Property(x => x.Price)
                .HasColumnType("MONEY")
                .IsRequired();

            builder.Property(x => x.DeliveryPrice)
                .HasColumnType("MONEY")
                .IsRequired();

            builder
                .OwnsMany(x => x.ProductOptions, navigationBuilder =>
                {
                    navigationBuilder.ToTable("ProductOptions", ProductsContext.DefaultSchema);
                    
                    navigationBuilder.HasKey(x => x.Id);
                    navigationBuilder.Property(x => x.Id)
                        .ValueGeneratedNever();

                    navigationBuilder.Property(x => x.Name)
                        .IsRequired();

                    navigationBuilder.Property(x => x.Description)
                        .HasColumnType("TEXT")
                        .IsRequired(false);
                });
        }
    }
}
