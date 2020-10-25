using System.Collections.Generic;
using Products.Domain.Aggregates.ProductAggregate;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.TestData.Products
{
    public class ProductListTestData : TheoryData<IList<Product>>
    {
        public ProductListTestData()
        {
            Add(new List<Product> { CreateProduct(), CreateProduct() });
        }
    }
}