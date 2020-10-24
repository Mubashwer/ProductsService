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

        public async Task<IPagedList<ProductDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPagedAsync(pageNumber, pageSize);
            var productDtos = new PagedList<ProductDto>(products, products.Select(x => x.ToDto()));

            _logger.LogDebug("Products fetched successfully");
            return productDtos;
        }

        public async Task<ProductDto?> FindByIdAsync(Guid productId)
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

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var product = await _productRepository.AddAsync(productDto.ToEntity());
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} added successfully", product);
            return product.ToDto();
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            _productRepository.Update(product);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} updated successfully", product);
        }

        public async Task DeleteAsync(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            _productRepository.Delete(product);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{Product} deleted successfully", product);
        }
    }
}
