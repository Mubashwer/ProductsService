using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

            return productDtos;
        }

        public async Task<IPagedList<ProductOptionDto>?> GetPagedProductOptionsAsync(Guid productId, int pageNumber,
            int pageSize)
        {
            var product = await FindProduct(productId);
            if (product is null) return null;

            var productOptions = await product.ProductOptions.ToPagedListAsync(pageNumber, pageSize);
            var productOptionDtos =
                new PagedList<ProductOptionDto>(productOptions, productOptions.Select(x => x.ToDto(productId)));

            return productOptionDtos;
        }

        public async Task<ProductDto?> FindProductByIdAsync(Guid productId)
        {
            var product = await FindProduct(productId);
            return product?.ToDto();
        }

        public async Task<ProductOptionDto?> FindProductOptionByIdAsync(Guid productId, Guid productOptionId)
        {
            var (_, productOption) = await FindProductAndProductOption(productId, productOptionId);
            return productOption?.ToDto(productId);
        }

        public async Task<ProductDto> AddProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.AddAsync(productDto.ToEntity());
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{ProductDto} added", productDto);
            return product.ToDto();
        }

        public async Task<ProductOptionDto?> AddProductOptionAsync(Guid productId, ProductOptionDto productOptionDto)
        {
            var product = await FindProduct(productId);
            if (product is null) return null;

            product.AddProductOption(productOptionDto.Id, productOptionDto.Name, productOptionDto.Description);

            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("@{ProductOptionDto} added", productOptionDto);
            return product.ProductOptions.First(x => x.Id == productOptionDto.Id).ToDto(productId);
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var product = productDto.ToEntity();
            _productRepository.Update(product);

            try
            {
                await _productRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _productRepository.FindByIdAsync(productDto.Id) is null)
                {
                    return false;
                }

                throw;
            }

            _logger.LogInformation("@{ProductDto} updated", productDto);
            return true;
        }

        public async Task<bool> UpdateProductOptionAsync(Guid productId, ProductOptionDto productOptionDto)
        {
            var (product, productOption) = await FindProductAndProductOption(productId, productOptionDto.Id);

            if (product is null || productOption is null) return false;

            productOption.Update(productOptionDto.Name, productOptionDto.Description);
            await _productRepository.UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await FindProduct(productId);
            if (product is null) return false;

            _productRepository.Delete(product);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("Product with {ProductId} is deleted", productId);
            return true;
        }

        public async Task<bool> DeleteProductOptionAsync(Guid productId, Guid productOptionId)
        {
            var (product, productOption) = await FindProductAndProductOption(productId, productOptionId);

            if (product is null || productOption is null) return false;

            product.RemoveProductOption(productOption);
            await _productRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("ProductOption with {ProductOptionId} in Product with {ProductId} is deleted",
                productOptionId, productId);
            return true;
        }

        private async Task<Product?> FindProduct(Guid productId)
        {
            var product = await _productRepository.FindByIdAsync(productId);

            if (product is null)
            {
                _logger.LogInformation("Product with {ProductId} is not found", productId);
                return null;
            }

            return product;
        }

        private async Task<Tuple<Product?, ProductOption?>> FindProductAndProductOption(Guid productId,
            Guid productOptionId)
        {
            var product = await _productRepository.FindByIdAsync(productId);

            if (product is null)
            {
                _logger.LogInformation("Product with {ProductId} is not found", productId);
                return new Tuple<Product?, ProductOption?>(null, null);
            }

            var productOption = product.ProductOptions.FirstOrDefault(x => x.Id == productOptionId);

            if (productOption is null)
            {
                _logger.LogInformation("ProductOption with {ProductOptionId} is not found in Product with {ProductId}",
                    productOptionId, productId);
                return new Tuple<Product?, ProductOption?>(product, null);
            }

            return new Tuple<Product?, ProductOption?>(product, productOption);
        }
    }
}
