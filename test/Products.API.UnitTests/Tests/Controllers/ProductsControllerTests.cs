using System;
using System.Collections.Generic;
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
    public sealed class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _sut;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _sut = new ProductsController(_mockProductService.Object);
        }

        [Theory]
        [ClassData(typeof(ProductListTestData))]
        public async Task Get_ProductServiceReturnsProductDtos_ReturnsProductDtos(IList<Product> products)
        {
            //Arrange
            const int pageNumber = 1;
            const int pageSize = 100;
            var productDtos = products.Select(x => x.ToDto()).ToPagedList(pageSize, pageNumber);

            _mockProductService
                .Setup(x => x.GetPagedProductsAsync(pageNumber, pageSize))
                .ReturnsAsync(productDtos)
                .Verifiable();

            //Act
            var result = await _sut.Get();

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productDtos.Count, result.Items.Count());
        }

        [Fact]
        public async Task GetById_AndProductServiceReturnsProductDto_ReturnsProductDto()
        {
            //Arrange
            var productDto = CreateProduct().ToDto();
            _mockProductService.Setup(x => x.GetProductAsync(productDto.Id)).ReturnsAsync(productDto).Verifiable();

            //Act
            var result = await _sut.Get(productDto.Id);

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productDto, result.Value);
        }

        [Fact]
        public async Task GetById_AndProductServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            var productDto = CreateProduct().ToDto();
            _mockProductService
                .Setup(x => x.GetProductAsync(productDto.Id))
                .ReturnsAsync((ProductDto?)null)
                .Verifiable();

            //Act
            var result = await _sut.Get(productDto.Id);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ProductServiceAddsProduct_ReturnsCreatedProductDto()
        {
            //Arrange
            var productDto = CreateProduct().ToDto();
            _mockProductService.Setup(x => x.AddProductAsync(productDto)).ReturnsAsync(productDto).Verifiable();

            //Act
            var result = await _sut.Post(productDto);

            //Assert
            _mockProductService.Verify();
            Assert.Equal(productDto, ((ObjectResult)result.Result).Value);
        }

        [Fact]
        public async Task Put_ProductServiceReturnsTrueWhenUpdatingProduct_ReturnsNoContent()
        {
            //Arrange
            var productDto = CreateProduct().ToDto();
            _mockProductService.Setup(x => x.UpdateProductAsync(productDto)).ReturnsAsync(true).Verifiable();

            //Act
            var result = await _sut.Put(productDto.Id, productDto);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ProductServiceReturnsFalseWhenUpdatingProduct_ReturnsNotFound()
        {
            //Arrange
            var productDto = CreateProduct().ToDto();
            _mockProductService.Setup(x => x.UpdateProductAsync(productDto)).ReturnsAsync(false).Verifiable();

            //Act
            var result = await _sut.Put(productDto.Id, productDto);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ProductServiceReturnsTrueWhenDeletingProduct_ReturnsNoContent()
        {
            //Arrange
            var productId = Guid.NewGuid();
            _mockProductService.Setup(x => x.DeleteProductAsync(productId)).ReturnsAsync(true).Verifiable();

            //Act
            var result = await _sut.Delete(productId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ProductServiceReturnsFalseWhenDeletingProduct_ReturnsNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();
            _mockProductService.Setup(x => x.DeleteProductAsync(productId)).ReturnsAsync(false).Verifiable();

            //Act
            var result = await _sut.Delete(productId);

            //Assert
            _mockProductService.Verify();
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
