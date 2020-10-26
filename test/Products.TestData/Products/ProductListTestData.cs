using System;
using System.Collections.Generic;
using Products.Domain.Aggregates.ProductAggregate;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.TestData.Products
{
    public class ProductListTestData : TheoryData<IList<Product>>
    {
        private readonly Random _random = new Random();

        public ProductListTestData()
        {
            var productCount = _random.Next(1, 10);
            var productOptionCount = _random.Next(10);

            var productsList = new List<Product>();
            while (productCount-- != 0)
            {
                productsList.Add(CreateProduct(productOptionCount));
            }

            Add(productsList);
        }
    }
}