using System;
using System.Threading.Tasks;
using Products.API.Application.Dtos;
using X.PagedList;

namespace Products.API.Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        /// <summary>
        /// Gets paged list of products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>paged list of products</returns>
        Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Gets paged list of product options of a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>paged list of product options</returns>
        Task<IPagedList<ProductOptionDto>?> GetPagedProductOptionsAsync(Guid productId, int pageNumber,
            int pageSize);

        /// <summary>
        /// Finds product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Added <see cref="ProductOptionDto"/> if found; null otherwise</returns>
        Task<ProductDto?> GetProductAsync(Guid productId);

        /// <summary>
        /// Finds product option in product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns>Added <see cref="ProductOptionDto"/> if found; null otherwise</returns>
        Task<ProductOptionDto?> GetProductOptionAsync(Guid productId, Guid productOptionId);

        /// <summary>
        /// Adds product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns>Added <see cref="ProductDto"/></returns>
        Task<ProductDto> AddProductAsync(ProductDto productDto);
        
        /// <summary>
        /// Adds product option to a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionDto"></param>
        /// <returns>Added <see cref="ProductOptionDto"/> if product is found; null otherwise</returns>
        Task<ProductOptionDto?> AddProductOptionAsync(Guid productId, ProductOptionDto productOptionDto);

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns>true if updated; false if product is not found</returns>
        Task<bool> UpdateProductAsync(ProductDto productDto);

        /// <summary>
        /// Updates product option
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionDto"></param>
        /// <returns>true if updated; false if product or product option is not found</returns>
        Task<bool> UpdateProductOptionAsync(Guid productId, ProductOptionDto productOptionDto);

        /// <summary>
        /// Deletes product by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>true if deleted; false if product is not found</returns>
        Task<bool> DeleteProductAsync(Guid productId);

        /// <summary>
        /// Deletes product by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns>true if deleted; false if product or product option is not found</returns>
        Task<bool> DeleteProductOptionAsync(Guid productId, Guid productOptionId);
    }
}
