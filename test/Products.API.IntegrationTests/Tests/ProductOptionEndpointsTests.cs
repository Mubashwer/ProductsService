using System.Net;
using System.Threading.Tasks;
using Products.API.IntegrationTests.TestHelpers;
using Products.API.IntegrationTests.TestHelpers.Extensions;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.API.IntegrationTests.Tests
{
    public sealed class ProductOptionEndpointsTests : IClassFixture<TestApiFactory<Startup>>
    {
        private readonly TestApiFactory<Startup> _factory;
        public ProductOptionEndpointsTests(TestApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostProductOption_ProductExistsButProductOptionDoesNot_ReturnsAddedProductOptionWithStatus201()
        {
            // Arrange
            var dbProduct = CreateProduct();
            var productOptionDto = CreateProductOptionDto(dbProduct.Id);

            var payload = productOptionDto.ToJsonContent();
            using var client = _factory.CreateClientWithInMemoryDb(async productsContext =>
            {
                await productsContext.Products.AddAsync(dbProduct);
                await productsContext.SaveChangesAsync();
            });

            // Act
            using var response = await client.PostAsync($"api/products/{dbProduct.Id}/options", payload);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(await payload.ReadAsStringAsync(), await response.Content.ReadAsStringAsync());
        }

        //TODO: Add more tests later. Not adding more tests now due to time constraint
    }
}
