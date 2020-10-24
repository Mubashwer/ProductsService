using System;
using System.Threading.Tasks;
using Products.API.Application.Dtos;
using X.PagedList;

namespace Products.API.Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize);
        Task<IPagedList<ProductOptionDto>?> GetPagedProductOptionsAsync(Guid productId, int pageNumber,
            int pageSize);

        Task<ProductDto?> FindProductByIdAsync(Guid productId);
        Task<ProductDto> AddProductAsync(ProductDto productDto);
        Task UpdateProductAsync(ProductDto productDto);
        Task DeleteProductAsync(ProductDto productDto);
    }
}
