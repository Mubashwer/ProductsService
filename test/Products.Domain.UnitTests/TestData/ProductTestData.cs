using System;
using AutoFixture;
using Products.Domain.Aggregates.ProductAggregate;
using Xunit;

namespace Products.Domain.UnitTests.TestData
{
    public class ProductWithOptionsTestData : TheoryData<Product>
    {
        public ProductWithOptionsTestData()
        {
            var fixture = new Fixture();

            var product = new Product(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>(),
                Math.Abs(fixture.Create<decimal>()), Math.Abs(fixture.Create<decimal>()));

            product.AddProductOption(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>());
            product.AddProductOption(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>());

            Add(product);
        }
    }
}
