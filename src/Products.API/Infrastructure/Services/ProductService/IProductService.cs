using System;
using System.Threading.Tasks;
using Products.API.Application.Dtos;
using X.PagedList;

namespace Products.API.Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<IPagedList<ProductDto>> GetManyAsync(int pageNumber, int pageSize);
        Task<ProductDto?> FindByIdAsync(Guid productId);
        Task<ProductDto> AddAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(ProductDto productDto);
    }
}
