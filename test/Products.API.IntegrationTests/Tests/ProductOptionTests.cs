using System.Net;
using System.Threading.Tasks;
using Products.API.IntegrationTests.TestHelpers;
using Products.API.IntegrationTests.TestHelpers.Extensions;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.API.IntegrationTests.Tests
{
    public sealed class ProductOptionTests : IClassFixture<TestApiFactory<Startup>>
    {
        private readonly TestApiFactory<Startup> _factory;
        public ProductOptionTests(TestApiFactory<Startup> factory)
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
            var client = _factory.CreateClientWithInMemoryDb(async productsContext =>
            {
                await productsContext.Products.AddAsync(dbProduct);
                await productsContext.SaveChangesAsync();
            });

            // Act
            var response = await client.PostAsync($"products/{dbProduct.Id}/options", payload);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(await payload.ReadAsStringAsync(), await response.Content.ReadAsStringAsync());
        }

        //TODO: Add more tests later. Not adding more tests now due to time constraint
    }
}
