using System;
using AutoFixture;
using Products.API.Application.Dtos;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.TestData.Products
{
    public static class Helper 
    {
        public static Product CreateProduct(int optionsCount = 0)
        {
            var fixture = new Fixture();
            var product = new Product(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>(),
                Math.Abs(fixture.Create<decimal>()), Math.Abs(fixture.Create<decimal>()));

            var count = optionsCount;
            while (count > 0)
            {
                product.AddProductOption(fixture.Create<Guid>(), fixture.Create<string>(), fixture.Create<string>());
                count--;
            }
            
            return product;
        }

        public static ProductOptionDto CreateProductOptionDto(Guid productId)
        {
            var fixture = new Fixture();

            return new ProductOptionDto
            {
                Id = Guid.NewGuid(),
                Name = fixture.Create<string>(),
                Description = fixture.Create<string>(),
                ProductId = productId
            };
        }
    }
}
