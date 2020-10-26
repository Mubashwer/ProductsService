using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.API.Application.Dtos;
using Products.API.Controllers;
using Products.API.Extensions;
using Products.API.Infrastructure.Services.ProductService;
using Products.Domain.Aggregates.ProductAggregate;
using Products.TestData.Products;
using X.PagedList;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.API.UnitTests.Tests.Controllers
{
    public sealed class ProductOptionsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductOptionsController _sut;

        public ProductOptionsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _sut = new ProductOptionsController(_mockProductService.Object);
        }

        [Theory]
        [ClassData(typeof(ProductWithManyOptionsTestData))]
        public async Task GetByProductId_ProductServiceReturnsProductOptionDtos_ReturnsProductOptionDtos(Product product)
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;

            var productOptionDtos = product.ProductOptions
                .Select(x => x.ToDto(product.Id))
                .ToPagedList(pageNumber, pageSize);

            _mockProductService
                .Setup(x => x.GetPagedProductOptionsAsync(product.Id, pageNumber, pageSize))
                .ReturnsAsync(productOptionDtos)
                .Verifiable();

            //Act
            var result = await _sut.Get(product.Id);

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productOptionDtos.Count, result.Value.Items.Count());
        }

        [Fact]
        public async Task GetByProductId_ProductServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;
            var productId = Guid.NewGuid();

            _mockProductService
                .Setup(x => x.GetPagedProductOptionsAsync(productId, pageNumber, pageSize))
                .ReturnsAsync((IPagedList<ProductOptionDto>?)null)
                .Verifiable();

            //Act
            var result = await _sut.Get(productId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByProductIdAndProductOptionId_ProductServiceReturnsProductOptionDto_ReturnsProductOptionDto()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductService
                .Setup(x => x.GetProductOptionAsync(productId, productOptionDto.Id))
                .ReturnsAsync(productOptionDto)
                .Verifiable();

            //Act
            var result = await _sut.Get(productId, productOptionDto.Id);

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productOptionDto, result.Value);
        }

        [Fact]
        public async Task GetByProductIdAndProductOptionId_ProductServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            _mockProductService
                .Setup(x => x.GetProductOptionAsync(productId, productOptionId))
                .ReturnsAsync((ProductOptionDto?)null)
                .Verifiable();

            //Act
            var result = await _sut.Get(productId, productOptionId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ProductServiceAddsProductOptionToProduct_ReturnsCreatedProductOption()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductService
                .Setup(x => x.AddProductOptionAsync(productOptionDto))
                .ReturnsAsync(productOptionDto)
                .Verifiable();

            //Act
            var result = await _sut.Post(productId, productOptionDto);

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productOptionDto, ((ObjectResult)result.Result).Value);
        }

        [Fact]
        public async Task Post_ProductServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductService
                .Setup(x => x.AddProductOptionAsync(productOptionDto))
                .ReturnsAsync((ProductOptionDto?)null)
                .Verifiable();

            //Act
            var result = await _sut.Post(productId, productOptionDto);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Put_ProductServiceReturnsTrueWhenUpdatingProductOption_ReturnsNoContent()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductService
                .Setup(x => x.UpdateProductOptionAsync(productOptionDto))
                .ReturnsAsync(true)
                .Verifiable();

            //Act
            var result = await _sut.Put(productId, productOptionDto.Id, productOptionDto);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ProductServiceReturnsFalseWhenUpdatingProductOption_ReturnsNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionDto = CreateProductOptionDto(productId);

            _mockProductService
                .Setup(x => x.UpdateProductOptionAsync(productOptionDto))
                .ReturnsAsync(false)
                .Verifiable();

            //Act
            var result = await _sut.Put(productId, productOptionDto.Id, productOptionDto);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ProductServiceReturnsTrueWhenDeletingProductOption_ReturnsNoContent()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            _mockProductService
                .Setup(x => x.DeleteProductOptionAsync(productId, productOptionId))
                .ReturnsAsync(true)
                .Verifiable();

            //Act
            var result = await _sut.Delete(productId, productOptionId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ProductServiceReturnsFalseWhenDeletingProductOption_ReturnsNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var productOptionId = Guid.NewGuid();

            _mockProductService
                .Setup(x => x.DeleteProductOptionAsync(productId, productOptionId))
                .ReturnsAsync(false)
                .Verifiable();

            //Act
            var result = await _sut.Delete(productId, productOptionId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
