using System;
using AutoFixture;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Domain.Commands.ProductAggregate;
using Xunit;

namespace Products.Domain.UnitTests.TestData
{
    public class ProductWithOptionsTestData : TheoryData<Product>
    {
        public ProductWithOptionsTestData()
        {
            var fixture = new Fixture();
            var product = new Product(new CreateProductCommand
            {
                ProductId = fixture.Create<Guid>(),
                Name = fixture.Create<string>(),
                Description = fixture.Create<string>(),
                DeliveryPrice = Math.Abs(fixture.Create<decimal>()),
                Price = Math.Abs(fixture.Create<decimal>()),
            });

            product.AddProductOption(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>());
            product.AddProductOption(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>());

            Add(product);
        }
    }
}
