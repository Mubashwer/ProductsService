using System;
using FluentValidation;
using Products.Domain.Commands.ProductAggregate;
using Products.Domain.Common;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public class ProductOption : Entity
    {
        public ProductOption(Guid id, string name, string? description) : base(id)
        {
            var command = new CreateProductOptionCommand
            {
                ProductOptionId = id,
                Name = name,
                Description = description
            };

            var validator = new CreateProductOptionCommandValidator();
            validator.ValidateAndThrow(command);

            Name = command.Name;
            Description = command.Description;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }

        public void Update(string name, string? description)
        {
            var command = new UpdateProductOptionCommand
            {
                Name = name,
                Description = description
            };
            
            var validator = new UpdateProductOptionCommandValidator();
            validator.ValidateAndThrow(command);

            Name = command.Name;
            Description = command.Description;
        }
    }
}
