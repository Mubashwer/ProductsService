using FluentValidation;
using Products.Domain.Commands.ProductAggregate;
using Products.Domain.Common;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public class ProductOption : Entity
    {
        public ProductOption(CreateProductOptionCommand command) : base(command.ProductOptionId)
        {
            var validator = new CreateProductOptionCommandValidator();
            validator.ValidateAndThrow(command);
            Name = command.Name;
            Description = command.Description;
            Product = command.Product;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Product Product { get; }

        public void Update(UpdateProductOptionCommand command)
        {
            var validator = new UpdateProductOptionCommandValidator();
            validator.ValidateAndThrow(command);
            Name = command.Name;
            Description = command.Description;
        }
    }
}
