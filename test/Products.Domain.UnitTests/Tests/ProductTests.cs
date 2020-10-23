using System;
using System.Linq;
using AutoFixture;
using FluentValidation;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Domain.Exceptions;
using Products.Domain.UnitTests.TestData;
using Xunit;

namespace Products.Domain.UnitTests.Tests
{
    public class ProductTests
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

        [Theory]
        [ClassData(typeof(ProductWithOptionsTestData))]
        public void AddProductOption_ProductAlreadyContainsProductOption_ThrowsProductDomainException(Product sut)
        {
            //Arrange
            var fixture = new Fixture();
            var productOption = sut.ProductOptions.First();

            //Act & Assert
            Assert.Throws<ProductsDomainException>(() =>
                sut.AddProductOption(productOption.Id, fixture.Create<string>(), fixture.Create<string>()));
        }

        //TODO: Add more tests later. Not adding more tests now due to time constraint
    }
}
