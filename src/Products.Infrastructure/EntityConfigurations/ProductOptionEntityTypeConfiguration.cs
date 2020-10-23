using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.Infrastructure.EntityConfigurations
{
    public class ProductOptionEntityTypeConfiguration : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("TEXT")
                .IsRequired(false);
        }
    }
}