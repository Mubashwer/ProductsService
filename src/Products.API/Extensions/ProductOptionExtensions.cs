using Products.API.Application.Dtos;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.API.Extensions
{
    public static class ProductOptionExtensions
    {
        public static ProductOptionDto ToDto(this ProductOption productOption)
        {
            return new ProductOptionDto
            {
                Id = productOption.Id,
                Name = productOption.Name,
                Description = productOption.Description,
            };
        }

        public static ProductOption ToEntity(this ProductOptionDto dto)
        {
            return new ProductOption(dto.Id, dto.Name, dto.Description);
        }
    }
}