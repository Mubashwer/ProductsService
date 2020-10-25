using System;
using System.Linq;
using AutoFixture;
using FluentValidation;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Domain.Exceptions;
using Xunit;
using static Products.TestData.Products.Helper;

namespace Products.Domain.UnitTests.Tests
{
    public sealed class ProductTests
    {
        [Fact]
        public void CreateProduct_WithNullName_ThrowsValidationExceptionWithEmptyNameError()
        {
            //Arrange
            var fixture = new Fixture();
            const string propertyName = "Name";
            const string errorMessage = "'Name' must not be empty.";
            
            //Act & Assert
            var exception = Assert.Throws<ValidationException>(() => new Product(fixture.Create<Guid>(), null!,
                fixture.Create<string>(),
                Math.Abs(fixture.Create<decimal>()), Math.Abs(fixture.Create<decimal>())));

            //Assert
            Assert.Contains(exception.Errors, x => x.PropertyName == propertyName && x.ErrorMessage == errorMessage);
        }

        [Fact]
        public void AddProductOption_ProductAlreadyContainsProductOption_ThrowsProductDomainException()
        {
            //Arrange
            var fixture = new Fixture();
            var product = CreateProduct(1);
            var productOption = product.ProductOptions.First();

            //Act & Assert
            Assert.Throws<ProductsDomainException>(() =>
                product.AddProductOption(productOption.Id, fixture.Create<string>(), fixture.Create<string>()));
        }

        //TODO: Add more tests later. Not adding more tests now due to time constraint
    }
}
