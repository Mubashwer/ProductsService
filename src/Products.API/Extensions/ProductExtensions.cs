using Products.API.Application.Dtos;
using Products.Domain.Aggregates.ProductAggregate;

namespace Products.API.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };
        }

        public static Product ToEntity(this ProductDto dto)
        {
            return new Product(dto.Id, dto.Name, dto.Description, dto.Price, dto.DeliveryPrice);
        }
    }
}
