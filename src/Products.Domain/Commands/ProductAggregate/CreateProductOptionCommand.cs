﻿using System;
using FluentValidation;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.Domain.Commands.ProductAggregate
{
    public class CreateProductOptionCommand
    {
        public Guid ProductOptionId { get; set; }
        public Product Product { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateProductOptionCommandValidator : AbstractValidator<CreateProductOptionCommand>
    {
        public CreateProductOptionCommandValidator()
        {
            RuleFor(x => x.ProductOptionId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}