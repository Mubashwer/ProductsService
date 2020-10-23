using FluentValidation;

namespace Products.Domain.Commands.ProductAggregate
{
    public class UpdateProductOptionCommand
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class UpdateProductOptionCommandValidator : AbstractValidator<UpdateProductOptionCommand>
    {
        public UpdateProductOptionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}