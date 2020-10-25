using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Products.API.Application.Dtos;
using Products.API.Extensions;
using Products.API.IntegrationTests.TestHelpers;
using Products.API.IntegrationTests.TestHelpers.Extensions;
using Products.Domain.Aggregates.ProductAggregate;
using Products.TestData.Products;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.API.IntegrationTests.Tests
{
    public sealed class ProductTests : IClassFixture<TestApiFactory<Startup>>
    {
        private readonly TestApiFactory<Startup> _factory;
        public ProductTests(TestApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [ClassData(typeof(ProductListTestData))]
        public async Task GetProducts_ProductsAvailable_ReturnsProductsWithStatus200(IList<Product> dbProducts)
        {
            // Arrange
            var client = _factory.CreateClientWithInMemoryDb(async productsContext =>
            {
                await productsContext.AddRangeAsync(dbProducts);
                await productsContext.SaveChangesAsync();
            });

            // Act
            using var response = await client.GetAsync("api/products");

            // Assert
            var responseProducts =
                JsonSerializer.Deserialize<PagedListDto<ProductDto>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(dbProducts.Count, responseProducts.Items.Count());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_ProductDoesNotAlreadyExist_ReturnsCreatedProductWithStatus201()
        {
            // Arrange
            var payload = CreateProduct().ToDto().ToJsonContent();
            var client = _factory.CreateClientWithInMemoryDb();

            // Act
            var response = await client.PostAsync("api/products", payload);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(await payload.ReadAsStringAsync(), await response.Content.ReadAsStringAsync());
        }

        //TODO: Add more tests later. Not adding more tests now due to time constraint
    }
}
