using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Products.API.Application.Dtos;
using Products.API.Extensions;
using Products.Domain.Aggregates.ProductAggregate;
using X.PagedList;

namespace Products.API.Infrastructure.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;

        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPagedAsync(pageNumber, pageSize);
            var productDtos = new PagedList<ProductDto>(products, products.Select(x => x.ToDto()));

            _logger.LogDebug("Products fetched successfully");
            return productDtos;
        }

        public async Task<IPagedList<ProductOptionDto>?> GetPagedProductOptionsAsync(Guid productId, int pageNumber,
            int pageSize)
        {
            var product = await _productRepository.FindByIdAsync(productId);

            if (product is null)
            {
                _logger.LogInformation("Product with {ProductId} not found", productId);
                return null;
            }

            var productOptions = await product.ProductOptions.ToPagedListAsync(pageNumber, pageSize);
            var productOptionDtos =
                new PagedList<ProductOptionDto>(productOptions, productOptions.Select(x => x.ToDto()));

            _logger.LogDebug("ProductOptions fetched successfully");
            return productOptionDtos;
        }

        public async Task<ProductDto?> FindProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.FindByIdAsync(productId);

            if (product is null)
            {
                _logger.LogInformation("Product with {ProductId} not found", productId);
            }
            else
            {
                _logger.LogDebug("{Product} found", product);
            }

            return product?.ToDto();
        }

        public async Task<ProductDto> AddProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.AddAsync(productDto.ToEntity());
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} added successfully", product);
            return product.ToDto();
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} updated successfully", product);
        }

        public async Task DeleteProductAsync(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            _productRepository.Delete(product);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} deleted successfully", product);
        }
    }
}
