using System;
using System.Collections.Generic;
using FluentValidation;
using Products.Domain.Commands.ProductAggregate;
using Products.Domain.Common;
using Products.Domain.Exceptions;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public Product(Guid id, string name, string? description, decimal price, decimal deliveryPrice) : base(id)
        {
            var command = new CreateProductCommand
            {
                ProductId = id,
                Name = name,
                Description = description,
                Price = price,
                DeliveryPrice = deliveryPrice
            };

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

        public void Update(string name, string? description, decimal price, decimal deliveryPrice)
        {
            var command = new UpdateProductCommand
            {
                Name = name,
                Description = description,
                Price = price,
                DeliveryPrice = deliveryPrice
            };

            var validator = new UpdateProductCommandValidator();
            validator.ValidateAndThrow(command);

            Name = command.Name;
            Description = command.Description;
            Price = command.Price;
            DeliveryPrice = command.DeliveryPrice;
        }

        public void AddProductOption(Guid productOptionId, string name, string? description)
        {
            var productOption = new ProductOption(productOptionId, name, description);

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
