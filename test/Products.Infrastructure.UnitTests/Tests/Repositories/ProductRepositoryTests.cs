using System;
using System.Collections.Generic;
using System.Linq;
using Products.Infrastructure.Persistence.Repositories;
using Products.Infrastructure.UnitTests.TestHelpers;
using System.Threading.Tasks;
using AutoFixture;
using Products.Domain.Aggregates.ProductAggregate;
using Products.TestData.Products;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.Infrastructure.UnitTests.Tests.Repositories
{
    public sealed class ProductRepositoryTests : InMemoryDatabaseTestBase
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(-1, -1)]
        public async Task GetPagedAsync_PageNumberOrPageSizeIsLessThanOne_ThrowsArgumentOutOfRangeException(
            int pageNumber, int pageSize)
        {
            //Arrange
            await using var dbContext = GetProductsContext();
            var sut = new ProductRepository(dbContext);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.GetPagedAsync(pageNumber, pageSize));
        }

        [Theory]
        [ClassData(typeof(ProductListTestData))]
        public async Task GetPagedAsync_PageNumberAndPageSizeAreBothGreaterThanOne_ReturnsProductsFromDatabase(IList<Product> products)
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;
            await using var dbContext = GetProductsContext();
            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();

            var sut = new ProductRepository(dbContext);

            //Act
            var result = await sut.GetPagedAsync(pageNumber, pageSize);

            //Assert
            Assert.Equal(products.AsEnumerable(), result.AsEnumerable());
        }

        [Fact]
        public async Task FindByIdAsync_ProductExistsInDatabase_ReturnsProduct()
        {
            //Arrange
            var product = CreateProduct();
            await using var dbContext = GetProductsContext();
            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var sut = new ProductRepository(dbContext);

            //Act
            var result = await sut.FindByIdAsync(product.Id);

            //Assert
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task FindByIdAsync_ProductDoesNotExistInDatabase_ReturnsNull()
        {
            //Arrange
            var productId = Guid.NewGuid();
            await using var dbContext = GetProductsContext();

            var sut = new ProductRepository(dbContext);

            //Act
            var result = await sut.FindByIdAsync(productId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_WhenSaved_AddsProductToDatabase()
        {
            //Arrange
            var product = CreateProduct();
            await using var dbContext = GetProductsContext();

            var sut = new ProductRepository(dbContext);

            //Act
            var result = await sut.AddAsync(product);
            await sut.UnitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(product, await dbContext.Products.FindAsync(product.Id));
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task UpdateAsync_WhenSaved_UpdatesProductInDatabase()
        {
            //Arrange
            var product = CreateProduct();
            await using var dbContext = GetProductsContext();
            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var fixture = new Fixture();
            product.Update(fixture.Create<string>(), product.Description, product.Price, product.DeliveryPrice);

            var sut = new ProductRepository(dbContext);

            //Act
            sut.Update(product);
            await sut.UnitOfWork.SaveChangesAsync();

            //Assert
            var updatedProduct = await dbContext.Products.FindAsync(product.Id);
            Assert.Equal(product.Name, updatedProduct.Name);
        }

        [Fact]
        public async Task DeleteAsync_WhenSaved_RemovesProductFromDatabase()
        {
            //Arrange
            var product = CreateProduct();
            await using var dbContext = GetProductsContext();
            await dbContext.AddAsync(product);

            var sut = new ProductRepository(dbContext);

            //Act
            sut.Delete(product);
            await sut.UnitOfWork.SaveChangesAsync();

            //Assert
            Assert.Null(await dbContext.Products.FindAsync(product.Id));
        }
    }
}
