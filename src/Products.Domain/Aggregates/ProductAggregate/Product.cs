using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Products.Domain.Commands.ProductAggregate;
using Products.Domain.Common;
using Products.Domain.Exceptions;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public Product(CreateProductCommand command) : base(command.ProductId)
        {
            var validator = new CreateProductCommandValidator();
            validator.ValidateAndThrow(command);

            Name = command.Name;
            Description = command.Description;
            Price = command.Price;
            DeliveryPrice = command.DeliveryPrice;
            _productOptions = new List<ProductOption>();
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal DeliveryPrice { get; private set; }

        private readonly List<ProductOption> _productOptions;
        public IReadOnlyCollection<ProductOption> ProductOptions => _productOptions;

        public void Update(UpdateProductCommand command)
        {
            var validator = new UpdateProductCommandValidator();
            validator.ValidateAndThrow(command);
            Name = command.Name;
            Description = command.Description;
            Price = command.Price;
            DeliveryPrice = command.DeliveryPrice;
        }

        public void AddProductOption(Guid productOptionId, string name, string description)
        {
            var createProductOptionCommand = new CreateProductOptionCommand
            {
                Product = this,
                Name = name,
                Description = description,
                ProductOptionId = productOptionId
            };

            var productOption = new ProductOption(createProductOptionCommand);

            if (_productOptions.Contains(productOption))
            {
                throw new ProductsDomainException("Product option is already available in product");
            }

            _productOptions.Add(productOption);
        }

        public void RemoveProductOption(ProductOption productOption)
        {
            _ = productOption ?? throw new ArgumentNullException(nameof(productOption));
            
            var result = _productOptions.Remove(productOption);
            if (!result)
            {
                throw  new ProductsDomainException("Product option not found in product");
            }
        }
    }
}
