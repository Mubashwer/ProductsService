using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Products.API.Extensions;
using Products.API.Infrastructure.Services.ProductService;
using Products.Domain.Aggregates.ProductAggregate;
using Products.TestData.Products;
using X.PagedList;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.API.UnitTests.Tests.Services
{
    public sealed class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductService _sut;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            var stubLogger = Mock.Of<ILogger<ProductService>>();
            _sut = new ProductService(stubLogger, _mockProductRepository.Object);
        }

        [Theory]
        [ClassData(typeof(ProductListTestData))]
        public async Task GetPagedProductsAsync_ProductRepositoryReturnsProducts_ReturnsProductDtos(
            IList<Product> products)
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;

            _mockProductRepository
                .Setup(x => x.GetPagedAsync(pageNumber, pageSize))
                .ReturnsAsync(products.ToPagedList())
                .Verifiable();

            //Act
            var result = await _sut.GetPagedProductsAsync(pageNumber, pageSize);

            //Assert
            _mockProductRepository.Verify();
            Assert.Equal(products.Count, result.Count);
        }

        [Theory]
        [ClassData(typeof(ProductWithManyOptionsTestData))]
        public async Task
            GetPagedProductOptionsAsync_ProductRepositoryReturnsProductWithProductOptions_ReturnsProductOptionsDtos(
                Product product)
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync(product)
                .Verifiable();

            //Act
            var result = await _sut.GetPagedProductOptionsAsync(product.Id, pageNumber, pageSize);

            //Arrange
            _mockProductRepository.Verify();
            Assert.Equal(product.ProductOptions.Count, result!.Count);
        }

        [Fact]
        public async Task GetProductAsync_ProductRepositoryReturnsProduct_ReturnsProductDto()
        {
            //Arrange
            var product = CreateProduct();

            _mockProductRepository.Setup(x => x.FindByIdAsync(product.Id)).ReturnsAsync(product).Verifiable();

            //Act
            var result = await _sut.GetProductAsync(product.Id);

            //Assert
            _mockProductRepository.Verify();
            Assert.Equal(JsonSerializer.Serialize(product.ToDto()), JsonSerializer.Serialize(result));
        }

        [Fact]
        public async Task GetProductAsync_ProductRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            var productId = Guid.NewGuid();
            _mockProductRepository.Setup(x => x.FindByIdAsync(productId)).ReturnsAsync((Product?)null).Verifiable();

            //Act
            var result = await _sut.GetProductAsync(productId);

            //Assert
            _mockProductRepository.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task
            GetProductOptionAsync_ProductRepositoryReturnsProductWithProductOptionForGivenId_ReturnsProductOptionDto()
        {
            //Arrange
            var product = CreateProduct(1);
            var productOption = product.ProductOptions.First();
            var productOptionDto = productOption.ToDto(product.Id);

            _mockProductRepository.Setup(x => x.FindByIdAsync(product.Id)).ReturnsAsync(product).Verifiable();

            //Act
            var result = await _sut.GetProductOptionAsync(product.Id, productOption.Id);

            //Assert
            _mockProductRepository.Verify();
            Assert.Equal(JsonSerializer.Serialize(productOptionDto), JsonSerializer.Serialize(result));
        }

        [Theory]
        [ClassData(typeof(ProductWithManyOptionsTestData))]
        public async Task
            GetProductOptionAsync_ProductRepositoryReturnsProductWithoutAnyProductOptionForGivenId_ReturnsNull(
                Product product)
        {
            //Arrange
            var productOptionId = Guid.NewGuid();
            _mockProductRepository.Setup(x => x.FindByIdAsync(product.Id)).ReturnsAsync(product).Verifiable();

            //Act
            var result = await _sut.GetProductOptionAsync(product.Id, productOptionId);

            //Assert
            _mockProductRepository.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductOptionAsync_ProductRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(productId))
                .ReturnsAsync((Product?)null)
                .Verifiable();

            //Act
            var result = await _sut.GetProductOptionAsync(productId, productOptionId);

            //Assert
            _mockProductRepository.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductAsync_ProductRepositoryAddsProduct_ReturnsCreatedProductDto()
        {
            //Arrange
            var product = CreateProduct();
            _mockProductRepository.Setup(x => x.AddAsync(product)).ReturnsAsync(product).Verifiable();

            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.AddProductAsync(product.ToDto());

            //Assert
            _mockProductRepository.Verify();
            Assert.Equal(JsonSerializer.Serialize(product.ToDto()), JsonSerializer.Serialize(result));
        }

        [Fact]
        public async Task AddProductOptionAsync_ProductRepositoryAddsProductOption_ReturnsCreatedProductOptionDto()
        {
            //Arrange
            var product = CreateProduct();
            var productOptionDto = CreateProductOptionDto(product.Id);

            _mockProductRepository.Setup(x => x.FindByIdAsync(product.Id)).ReturnsAsync(product).Verifiable();
            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.AddProductOptionAsync(productOptionDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.Equal(JsonSerializer.Serialize(productOptionDto), JsonSerializer.Serialize(result));
        }

        [Fact]
        public async Task AddProductOptionAsync_ProductRepositoryReturnsNullForGivenProductId_ReturnsNull()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(productId))
                .ReturnsAsync((Product?)null)
                .Verifiable();
            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            //Act
            var result = await _sut.AddProductOptionAsync(productOptionDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ProductRepositoryUpdatesProduct_ReturnsTrue()
        {
            //Arrange
            var updatedProductDto = CreateProduct().ToDto();

            _mockProductRepository.Setup(x => x.Update(updatedProductDto.ToEntity())).Verifiable();
            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.UpdateProductAsync(updatedProductDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ProductRepositoryCannotExceptionBecauseProductDoesNotExist_ReturnsFalse()
        {
            //Arrange
            var updatedProductDto = CreateProduct().ToDto();

            _mockProductRepository.Setup(x => x.Update(updatedProductDto.ToEntity())).Verifiable();

            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Throws<DbUpdateConcurrencyException>()
                .Verifiable();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(updatedProductDto.Id))
                .ReturnsAsync((Product?)null)
                .Verifiable();

            //Act
            var result = await _sut.UpdateProductAsync(updatedProductDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task
            UpdateProductOptionAsync_ProductRepositoryFindsProductWithProductOptionAndUpdatesIt_ReturnsTrue()
        {
            //Arrange
            var product = CreateProduct(1);

            var updatedProductOptionDto = CreateProductOptionDto(product.Id);
            updatedProductOptionDto.Id = product.ProductOptions.First().Id;

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync(product)
                .Verifiable();

            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.UpdateProductOptionAsync(updatedProductOptionDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task
            UpdateProductOptionAsync_ProductRepositoryFindsProductButDoesNotFindAnyProductOptionForGivenId_ReturnsFalse()
        {
            //Arrange
            var product = CreateProduct(1);
            var updatedProductOptionDto = CreateProductOptionDto(product.Id);

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(updatedProductOptionDto.ProductId))
                .ReturnsAsync(product)
                .Verifiable();

            //Act
            var result = await _sut.UpdateProductOptionAsync(updatedProductOptionDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task
            UpdateProductOptionAsync_ProductRepositoryReturnsNullForGivenProductId_ReturnsFalse()
        {
            //Arrange
            var updatedProductOptionDto = CreateProductOptionDto(Guid.NewGuid());

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(updatedProductOptionDto.ProductId))
                .ReturnsAsync((Product?)null)
                .Verifiable();

            //Act
            var result = await _sut.UpdateProductOptionAsync(updatedProductOptionDto);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductRepositoryFindsProductForGivenIdAndDeletesIt_ReturnsTrue()
        {
            //Arrange
            var product = CreateProduct();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync(product)
                .Verifiable();

            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.DeleteProductAsync(product.Id);

            //Assert
            _mockProductRepository.Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductRepositoryReturnsNullForGivenProductId_ReturnsFalse()
        {
            //Arrange
            var product = CreateProduct();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync((Product?)null)
                .Verifiable();

            //Act
            var result = await _sut.DeleteProductAsync(product.Id);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task
            DeleteProductOptionAsync_ProductRepositoryFindsProductWithProductOptionAndDeletesIt_ReturnsTrue()
        {
            //Arrange
            var product = CreateProduct(1);
            var productOptionId = product.ProductOptions.First().Id;

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync(product)
                .Verifiable();

            _mockProductRepository
                .Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Act
            var result = await _sut.DeleteProductOptionAsync(product.Id, productOptionId);

            //Assert
            _mockProductRepository.Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task
            DeleteProductOptionAsync_ProductRepositoryFindsProductButDoesNotFindAnyProductOptionForGivenId_ReturnsFalse()
        {
            //Arrange
            var product = CreateProduct();
            var productOptionId = Guid.NewGuid();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(product.Id))
                .ReturnsAsync(product)
                .Verifiable();

            //Act
            var result = await _sut.DeleteProductOptionAsync(product.Id, productOptionId);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task
            DeleteProductOptionAsync_ProductRepositoryReturnsNullForGivenProductId_ReturnsFalse()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            _mockProductRepository
                .Setup(x => x.FindByIdAsync(productId))
                .ReturnsAsync((Product?)null)
                .Verifiable();

            //Act
            var result = await _sut.DeleteProductOptionAsync(productId, productOptionId);

            //Assert
            _mockProductRepository.Verify();
            Assert.False(result);
        }
    }
}
