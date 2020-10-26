using System;
using Products.Domain.Aggregates.ProductAggregate;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.TestData.Products
{
    public class ProductWithManyOptionsTestData : TheoryData<Product>
    {
        private readonly Random _random = new Random();

        public ProductWithManyOptionsTestData()
        {
            var productOptionCount = _random.Next(2, 10);

            Add(CreateProduct(productOptionCount));
        }
    }
}