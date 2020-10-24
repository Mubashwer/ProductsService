using System;
using Products.API.Application.Dtos;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.API.Extensions
{
    public static class ProductOptionExtensions
    {
        public static ProductOptionDto ToDto(this ProductOption productOption, Guid productId)
        {
            return new ProductOptionDto
            {
                Id = productOption.Id,
                Name = productOption.Name,
                Description = productOption.Description,
                ProductId = productId
            };
        }
    }
}