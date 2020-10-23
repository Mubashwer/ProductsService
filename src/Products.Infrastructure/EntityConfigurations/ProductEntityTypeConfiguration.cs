using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.Infrastructure.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

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

            builder.HasMany(x => x.ProductOptions)
                .WithOne()
                .IsRequired(); // This ensures foreign key cannot be null and uses cascade delete behaviour
        }
    }
}
